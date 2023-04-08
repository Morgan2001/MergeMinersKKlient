using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Enums;
using MergeMiner.Core.State.Repository;
using Utils;

namespace _Proxy.Connectors
{
    public class MinerFieldConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly PlayerActionService _playerActionService;
        private readonly MinerRepository _minerRepository;
        private readonly MinerConfig _minerConfig;

        private ReactiveEvent<RelocateEvent> _resizeEvent = new();
        public IReactiveSubscription<RelocateEvent> ResizeEvent => _resizeEvent;
        
        private ReactiveEvent<AddMinerData> _addMinerEvent = new();
        public ReactiveEvent<AddMinerData> AddMinerEvent => _addMinerEvent;
        
        private ReactiveEvent<MergeMinersData> _mergeMinersEvent = new();
        public ReactiveEvent<MergeMinersData> MergeMinersEvent => _mergeMinersEvent;
        
        private ReactiveEvent<SwapMinersData> _swapMinersEvent = new();
        public ReactiveEvent<SwapMinersData> SwapMinersEvent => _swapMinersEvent;
        
        private ReactiveEvent<RemoveMinerData> _removeMinerEvent = new();
        public ReactiveEvent<RemoveMinerData> RemoveMinerEvent => _removeMinerEvent;
        
        public MinerFieldConnector(
            LocalPlayer localPlayer,
            EventSubscriptionService eventSubscriptionService,
            PlayerMinersRepository playerMinersRepository,
            PlayerActionService playerActionService,
            MinerRepository minerRepository,
            MinerConfig minerConfig)
        {
            _localPlayer = localPlayer;
            _eventSubscriptionService = eventSubscriptionService;
            _playerMinersRepository = playerMinersRepository;
            _playerActionService = playerActionService;
            _minerRepository = minerRepository;
            _minerConfig = minerConfig;

            _eventSubscriptionService.Subscribe<RelocateEvent>(_resizeEvent.Trigger);
            _eventSubscriptionService.Subscribe<AddMinerEvent>(OnAddMiner);
            _eventSubscriptionService.Subscribe<MergeMinersEvent>(OnMergeMiners);
            _eventSubscriptionService.Subscribe<SwapMinersEvent>(OnSwapMiners);
            _eventSubscriptionService.Subscribe<RemoveMinerEvent>(OnRemoveMiner);
        }
        
        private void OnAddMiner(AddMinerEvent gameEvent)
        {
            var miner = _minerRepository.Get(gameEvent.Miner);
            var minerConfig = _minerConfig.Get(miner.ConfigId);
            _addMinerEvent.Trigger(new AddMinerData(gameEvent.Miner, miner.ConfigId, minerConfig.Level, gameEvent.Source, gameEvent.Slot));
        }
        
        private void OnMergeMiners(MergeMinersEvent gameEvent)
        {
            var miner = _minerRepository.Get(gameEvent.NewMiner);
            var minerConfig = _minerConfig.Get(miner.ConfigId);
            _mergeMinersEvent.Trigger(new MergeMinersData(gameEvent.NewMiner, miner.ConfigId, minerConfig.Level, gameEvent.Slot, gameEvent.Merged));
        }
        
        private void OnSwapMiners(SwapMinersEvent gameEvent)
        {
            _swapMinersEvent.Trigger(new SwapMinersData(gameEvent.Slot1, gameEvent.Slot2));
        }
        
        private void OnRemoveMiner(RemoveMinerEvent gameEvent)
        {
            _removeMinerEvent.Trigger(new RemoveMinerData(gameEvent.Miner));
        }

        public bool Drop(string minerId, int slot)
        {
            var playerMiners = _playerMinersRepository.Get(_localPlayer.Id);
            var minerAtSlot = playerMiners.Miners[slot];
            
            var slotOfMiner = playerMiners.Miners.IndexOf(minerId);
            if (slotOfMiner == slot) return false;

            var miner1 = _minerRepository.Get(minerId);
            var miner2 = minerAtSlot != null ? _minerRepository.Get(minerAtSlot) : null;
            
            if (minerAtSlot == null || miner1.ConfigId != miner2.ConfigId)
            {
                return _playerActionService.Process(new SwapMinersPlayerAction(_localPlayer.Id, slotOfMiner, slot));
            }
            else
            {
                return _playerActionService.Process(new MergeMinersPlayerAction(_localPlayer.Id, slotOfMiner, slot));
            }
        }

        public void Remove(string minerId)
        {
            _playerActionService.Process(new RemoveMinerPlayerAction(_localPlayer.Id, minerId));
        }
    }

    public struct AddMinerData
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }
        public MinerSource Source { get; }
        public int Slot { get; }

        public AddMinerData(string id, string name, int level, MinerSource source, int slot)
        {
            Id = id;
            Name = name;
            Level = level;
            Source = source;
            Slot = slot;
        }
    }
    
    public struct MergeMinersData
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }
        public int Slot { get; }
        public string[] Merged { get; }

        public MergeMinersData(string id, string name, int level, int slot, string[] merged)
        {
            Id = id;
            Name = name;
            Level = level;
            Slot = slot;
            Merged = merged;
        }
    }
    
    public struct SwapMinersData
    {
        public int Slot1 { get; }
        public int Slot2 { get; }

        public SwapMinersData(int slot1, int slot2)
        {
            Slot1 = slot1;
            Slot2 = slot2;
        }
    }
    
    public struct RemoveMinerData
    {
        public string Miner { get; }

        public RemoveMinerData(string miner)
        {
            Miner = miner;
        }
    }
}