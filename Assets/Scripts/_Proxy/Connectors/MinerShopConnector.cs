using System.Collections.Generic;
using System.Linq;
using _Proxy.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Services;
using Utils;

namespace _Proxy.Connectors
{
    public class MinerShopConnector
    {
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly MinerShopConfig _minerShopConfig;
        private readonly MinerConfig _minerConfig;
        private readonly PlayerActionProxy _playerActionProxy;

        private ReactiveEvent<AddMinerShopData> _addMinerShopEvent = new();
        public IReactiveSubscription<AddMinerShopData> AddMinerShopEvent => _addMinerShopEvent;
        
        private ReactiveEvent<UpdateMinerShopData> _updateMinerShopEvent = new();
        public IReactiveSubscription<UpdateMinerShopData> UpdateMinerShopEvent => _updateMinerShopEvent;

        private Dictionary<MinerShopConfigItem, Currency> _shop = new();

        public MinerShopConnector(
            EventSubscriptionService eventSubscriptionService,
            MinerShopConfig minerShopConfig,
            MinerConfig minerConfig,
            PlayerActionProxy playerActionProxy)
        {
            _minerShopConfig = minerShopConfig;
            _minerConfig = minerConfig;
            _playerActionProxy = playerActionProxy;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
            _eventSubscriptionService.Subscribe<UpdateShopEvent>(OnUpdateShop);
        }

        public void BuyMiner(int level, Currency currency)
        {
            _playerActionProxy.BuyMiner(level, currency);
        }

        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            var level = gameEvent.Level;
            var filtered = _minerShopConfig.GetAll().Where(x => x.UnlockLevelGems <= level);
            foreach (var minerShop in filtered)
            {
                var minerConfig = _minerConfig.Get(minerShop.MinerConfig);
                
                var currency =
                    minerShop.UnlockLevel <= level ? Currency.Money :
                    minerShop.UnlockLevelAds <= level ? Currency.Ads :
                    minerShop.UnlockLevelGems <= level ? Currency.Gems : 
                    0;
                
                var price = 
                    currency == Currency.Money ? minerConfig.Price :
                    currency == Currency.Ads ? 0 :
                    currency == Currency.Gems ? minerConfig.PriceInGems :
                    0;
                
                if (!_shop.ContainsKey(minerShop))
                {
                    _addMinerShopEvent.Trigger(new AddMinerShopData(minerShop.Id, minerConfig.Level));
                    _updateMinerShopEvent.Trigger(new UpdateMinerShopData(minerShop.MinerConfig, currency, price));
                    _shop.Add(minerShop, currency);
                }
                else if (_shop[minerShop] != currency)
                {
                    _updateMinerShopEvent.Trigger(new UpdateMinerShopData(minerShop.MinerConfig, currency, price));
                }
            }
        }

        private void OnUpdateShop(UpdateShopEvent gameEvent)
        {
            var minerConfig = _minerShopConfig.Get(gameEvent.MinerConfig);
            var currency = _shop[minerConfig];
            if (currency == Currency.Money)
            {
                _updateMinerShopEvent.Trigger(new UpdateMinerShopData(gameEvent.MinerConfig, Currency.Money, gameEvent.Price));
            }
        }
    }

    public struct AddMinerShopData
    {
        public string Id { get; }
        public int Level { get; }

        public AddMinerShopData(string id, int level)
        {
            Id = id;
            Level = level;
        }
    }
    
    public struct UpdateMinerShopData
    {
        public string Id { get; }
        public Currency Currency { get; }
        public double Price { get; }

        public UpdateMinerShopData(string id, Currency currency, double price)
        {
            Id = id;
            Currency = currency;
            Price = price;
        }
    }
}