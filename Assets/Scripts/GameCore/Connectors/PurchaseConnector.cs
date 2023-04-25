using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Events;
using GameCore.Preloader;
using GameCore.Services;
using IAP;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class PurchaseConnector
    {
        private const string SUBSCRIPTION = "com.mogames.get.crypto.nft.game.subscription";

        private readonly SessionData _sessionData;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly PlayerSubscriptionRepository _playerSubscriptionRepository;
        private readonly IAPConfig _iapConfig;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<ProductsData> _addProductEvent = new();
        public IReactiveSubscription<ProductsData> AddProductEvent => _addProductEvent;
        
        private ReactiveEvent<string> _initPurchaseEvent = new();
        public IReactiveSubscription<string> InitPurchaseEvent => _initPurchaseEvent;

        private ReactiveEvent _subscriptionEvent = new();
        public IReactiveSubscription SubscriptionEvent => _subscriptionEvent;

        public PurchaseConnector(
            SessionData sessionData,
            PlayerActionProxy playerActionProxy,
            PlayerSubscriptionRepository playerSubscriptionRepository,
            IAPConfig iapConfig,
            EventSubscriptionService eventSubscriptionService)
        {
            _sessionData = sessionData;
            _playerActionProxy = playerActionProxy;
            _playerSubscriptionRepository = playerSubscriptionRepository;
            _iapConfig = iapConfig;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInit);
        }

        private void OnInit(InitGameEvent gameEvent)
        {
            var subscription = _playerSubscriptionRepository.Get(_sessionData.Token);
            _addProductEvent.Trigger(new ProductsData(
                _iapConfig.GetAll().Select(x => new ProductData(x.Id, x.Gems, x.Price, true)),
                Enumerable.Repeat(new ProductData(SUBSCRIPTION, 0, 0, false), 1),
                new DateTime(subscription.ActiveTill) > DateTime.Now));
        }

        public void InitPurchase(string product)
        {
            _initPurchaseEvent.Trigger(product);
        }
        
        public void InitSubscription()
        {
            _initPurchaseEvent.Trigger(SUBSCRIPTION);
        }

        public void Purchase(PurchaseResult data)
        {
            _playerActionProxy.Purchase(data.Product, data.Token);
        }
        
        public void Subscription(PurchaseResult data)
        {
            _subscriptionEvent.Trigger();
            _playerActionProxy.Subscription(data.Token);
        }
    }

    public class ProductsData
    {
        public List<ProductData> Products { get; }
        public List<ProductData> Subscriptions { get; }
        public bool SubscriptionActive { get; }

        public ProductsData(IEnumerable<ProductData> products, IEnumerable<ProductData> subscriptions, bool subscriptionActive)
        {
            SubscriptionActive = subscriptionActive;
            Products = new List<ProductData>(products);
            Subscriptions = new List<ProductData>(subscriptions);
        }
    }

    public class ProductData
    {
        public string Id { get; }
        public int Gems { get; }
        public double Price { get; }
        public bool Consumable { get; }

        public ProductData(string id, int gems, double price, bool consumable)
        {
            Id = id;
            Gems = gems;
            Price = price;
            Consumable = consumable;
        }
    }
}