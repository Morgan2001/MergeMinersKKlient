using System.Collections.Generic;
using System.Linq;
using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
using Utils;

namespace _Proxy.Connectors
{
    public class MinerShopConnector
    {
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly LocalPlayer _localPlayer;
        private readonly MinerShopConfig _minerShopConfig;
        private readonly MinerConfig _minerConfig;
        private readonly PlayerActionService _playerActionService;

        private ReactiveEvent<AddMinerShopData> _addMinerShopEvent = new();
        public IReactiveSubscription<AddMinerShopData> AddMinerShopEvent => _addMinerShopEvent;
        
        private ReactiveEvent<UpdateMinerShopData> _updateMinerShopEvent = new();
        public IReactiveSubscription<UpdateMinerShopData> UpdateMinerShopEvent => _updateMinerShopEvent;

        private Dictionary<MinerShopConfigItem, CurrencyType> _shop = new();

        public MinerShopConnector(
            LocalPlayer localPlayer,
            EventSubscriptionService eventSubscriptionService,
            MinerShopConfig minerShopConfig,
            MinerConfig minerConfig,
            PlayerActionService playerActionService)
        {
            _localPlayer = localPlayer;
            _minerShopConfig = minerShopConfig;
            _minerConfig = minerConfig;
            _playerActionService = playerActionService;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
            _eventSubscriptionService.Subscribe<UpdateShopEvent>(OnUpdateShop);
        }

        public void BuyMiner(string minerId, CurrencyType currency)
        {
            _playerActionService.Process(new BuyMinerPlayerAction(_localPlayer.Id, minerId, currency == CurrencyType.Ads, currency == CurrencyType.Gems));
        }

        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            var level = gameEvent.Level;
            var filtered = _minerShopConfig.GetAll().Where(x => x.UnlockLevelGems <= level);
            foreach (var minerShop in filtered)
            {
                var minerConfig = _minerConfig.Get(minerShop.MinerConfig);
                
                var currency =
                    minerShop.UnlockLevel <= level ? CurrencyType.Money :
                    minerShop.UnlockLevelAds <= level ? CurrencyType.Ads :
                    minerShop.UnlockLevelGems <= level ? CurrencyType.Gems :
                    CurrencyType.None;
                
                var price = 
                    currency == CurrencyType.Money ? minerConfig.Price :
                    currency == CurrencyType.Ads ? 0 :
                    currency == CurrencyType.Gems ? minerConfig.PriceInGems :
                    0;
                
                if (!_shop.ContainsKey(minerShop))
                {
                    _addMinerShopEvent.Trigger(new AddMinerShopData(minerShop.Id, minerShop.MinerConfig, minerConfig.Level));
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
            if (currency == CurrencyType.Money)
            {
                _updateMinerShopEvent.Trigger(new UpdateMinerShopData(gameEvent.MinerConfig, CurrencyType.Money, gameEvent.Price));
            }
        }
    }

    public struct AddMinerShopData
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }

        public AddMinerShopData(string id, string name, int level)
        {
            Id = id;
            Name = name;
            Level = level;
        }
    }
    
    public struct UpdateMinerShopData
    {
        public string Id { get; }
        public CurrencyType Currency { get; }
        public double Price { get; }

        public UpdateMinerShopData(string id, CurrencyType currency, double price)
        {
            Id = id;
            Currency = currency;
            Price = price;
        }
    }

    public enum CurrencyType
    {
        None,
        Money,
        Ads,
        Gems
    }
}