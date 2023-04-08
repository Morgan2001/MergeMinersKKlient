using System.Collections.Generic;
using System.Linq;
using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Utils;
using Utils;

namespace _Proxy.Connectors
{
    public class MinerShopConnector
    {
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly LocalPlayer _localPlayer;
        private readonly MinerShopConfig _minerShopConfig;
        private readonly MinerRepository _minerRepository;
        private readonly MinerConfig _minerConfig;
        private readonly MinerShopHelper _minerShopHelper;
        private readonly PlayerActionService _playerActionService;

        private ReactiveEvent<AddMinerShopData> _addMinerShopEvent = new();
        public IReactiveSubscription<AddMinerShopData> AddMinerShopEvent => _addMinerShopEvent;
        
        private ReactiveEvent<UpdateMinerShopData> _updateMinerShopEvent = new();
        public IReactiveSubscription<UpdateMinerShopData> UpdateMinerShopEvent => _updateMinerShopEvent;

        private List<MinerShopConfigItem> _currentList = new();

        public MinerShopConnector(
            LocalPlayer localPlayer,
            EventSubscriptionService eventSubscriptionService,
            MinerShopConfig minerShopConfig,
            MinerRepository minerRepository,
            MinerConfig minerConfig,
            MinerShopHelper minerShopHelper,
            PlayerActionService playerActionService)
        {
            _localPlayer = localPlayer;
            _minerShopConfig = minerShopConfig;
            _minerRepository = minerRepository;
            _minerConfig = minerConfig;
            _minerShopHelper = minerShopHelper;
            _playerActionService = playerActionService;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<AddMinerEvent>(OnAddMiner);
            _eventSubscriptionService.Subscribe<MergeMinersEvent>(OnMergeMiners);
            _eventSubscriptionService.Subscribe<UpdateShopEvent>(OnUpdateShop);
        }

        public void BuyMiner(string minerId)
        {
            _playerActionService.Process(new BuyMinerPlayerAction(_localPlayer.Id, minerId, false));
        }
        
        private void OnAddMiner(AddMinerEvent gameEvent)
        {
            var miner = _minerRepository.Get(gameEvent.Miner);
            var minerConfig = _minerConfig.Get(miner.ConfigId);
            CheckShopLevel(minerConfig.Level);
        }
        
        private void OnMergeMiners(MergeMinersEvent gameEvent)
        {
            var miner = _minerRepository.Get(gameEvent.NewMiner);
            var minerConfig = _minerConfig.Get(miner.ConfigId);
            CheckShopLevel(minerConfig.Level);
        }

        private void CheckShopLevel(int level)
        {
            var filtered = _minerShopConfig.GetAll().Where(x => x.UnlockLevel <= level);
            foreach (var minerShop in filtered)
            {
                if (_currentList.Contains(minerShop)) continue;

                var minerConfig = _minerConfig.Get(minerShop.MinerConfig);
                var price = _minerShopHelper.GetMinerPrice(_localPlayer.Id, minerShop.MinerConfig);
                _addMinerShopEvent.Trigger(new AddMinerShopData(minerShop.Id, minerShop.MinerConfig, minerConfig.Level, price));
                _currentList.Add(minerShop);
            }
        }

        private void OnUpdateShop(UpdateShopEvent gameEvent)
        {
            // TODO: ads
            _updateMinerShopEvent.Trigger(new UpdateMinerShopData(gameEvent.MinerConfig, gameEvent.Price, false));
        }
    }

    public struct AddMinerShopData
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }
        public double Price { get; }

        public AddMinerShopData(string id, string name, int level, double price)
        {
            Id = id;
            Name = name;
            Level = level;
            Price = price;
        }
    }
    
    public struct UpdateMinerShopData
    {
        public string Id { get; }
        public double Price { get; }
        public bool Ads { get; }

        public UpdateMinerShopData(string id, double price, bool ads)
        {
            Id = id;
            Price = price;
            Ads = ads;
        }
    }
}