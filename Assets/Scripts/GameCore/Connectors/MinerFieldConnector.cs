﻿using GameCore.Events;
using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.PlayerActions.Base;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Enums;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Utils;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class MinerFieldConnector
    {
        private readonly SessionData _sessionData;
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly MinerRepository _minerRepository;
        private readonly MinerConfig _minerConfig;
        private readonly LocationHelper _locationHelper;
        private readonly BonusHelper _bonusHelper;

        private ReactiveEvent<RelocateData> _resizeEvent = new();
        public IReactiveSubscription<RelocateData> ResizeEvent => _resizeEvent;
        
        private ReactiveEvent<AddMinerData> _addMinerEvent = new();
        public ReactiveEvent<AddMinerData> AddMinerEvent => _addMinerEvent;
        
        private ReactiveEvent<MergeMinersData> _mergeMinersEvent = new();
        public ReactiveEvent<MergeMinersData> MergeMinersEvent => _mergeMinersEvent;
        
        private ReactiveEvent<SwapMinersData> _swapMinersEvent = new();
        public ReactiveEvent<SwapMinersData> SwapMinersEvent => _swapMinersEvent;
        
        private ReactiveEvent<RemoveMinerData> _removeMinerEvent = new();
        public ReactiveEvent<RemoveMinerData> RemoveMinerEvent => _removeMinerEvent;
        
        public MinerFieldConnector(
            SessionData sessionData,
            EventSubscriptionService eventSubscriptionService,
            PlayerMinersRepository playerMinersRepository,
            PlayerActionProxy playerActionProxy,
            MinerRepository minerRepository,
            MinerConfig minerConfig,
            LocationHelper locationHelper,
            BonusHelper bonusHelper)
        {
            _sessionData = sessionData;
            _playerMinersRepository = playerMinersRepository;
            _playerActionProxy = playerActionProxy;
            _minerRepository = minerRepository;
            _minerConfig = minerConfig;
            _locationHelper = locationHelper;
            _bonusHelper = bonusHelper;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInit);
            _eventSubscriptionService.Subscribe<RelocateEvent>(OnRelocate);
            _eventSubscriptionService.Subscribe<AddMinerEvent>(OnAddMiner);
            _eventSubscriptionService.Subscribe<MergeMinersEvent>(OnMergeMiners);
            _eventSubscriptionService.Subscribe<SwapMinersEvent>(OnSwapMiners);
            _eventSubscriptionService.Subscribe<RemoveMinerEvent>(OnRemoveMiner);
            _eventSubscriptionService.Subscribe<UseBonusEvent>(OnUseBonus);
            _eventSubscriptionService.Subscribe<EndBonusEvent>(OnEndBonus);
        }
        
        private void OnInit(InitGameEvent gameEvent)
        {
            Relocate(_sessionData.Token);

            var miners = _playerMinersRepository.Get(_sessionData.Token);
            var location = _locationHelper.GetLocation(_sessionData.Token);
            var bonusSlots = _bonusHelper.IsBonusActive(_sessionData.Token, BoostType.Slots) ? location.Width : 0; 
            var size = miners.Size + bonusSlots;
            for (var i = 0; i < size; i++)
            {
                var minerId = miners.Miners[i];
                if (minerId == null) continue;
                
                var miner = _minerRepository.Get(minerId);
                var minerConfig = _minerConfig.Get(miner.ConfigId);
                _addMinerEvent.Trigger(new AddMinerData(miner.Id, miner.ConfigId, minerConfig.Level, MinerSource.None, i));
            }
        }

        private void Relocate(string playerId)
        {
            var location = _locationHelper.GetLocation(playerId);
            var hasBonusSlots = _bonusHelper.IsBonusActive(playerId, BoostType.Slots);
            var bonusSlots = hasBonusSlots ? location.Width : 0; 
            var poweredSlots = _bonusHelper.IsBonusActive(playerId, BoostType.PowerAll)
                ? location.TotalSlots + bonusSlots
                : location.PoweredSlots;

            _resizeEvent.Trigger(new RelocateData(location.Width, hasBonusSlots ? location.Height + 1 : location.Height, poweredSlots));
        }

        private void OnRelocate(RelocateEvent gameEvent)
        {
            Relocate(gameEvent.Player);
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
            _mergeMinersEvent.Trigger(new MergeMinersData(gameEvent.NewMiner, minerConfig.Level, gameEvent.Slot, gameEvent.Merged, gameEvent.MaxLevelIncreased));
        }
        
        private void OnSwapMiners(SwapMinersEvent gameEvent)
        {
            _swapMinersEvent.Trigger(new SwapMinersData(gameEvent.Slot1, gameEvent.Slot2));
        }
        
        private void OnRemoveMiner(RemoveMinerEvent gameEvent)
        {
            _removeMinerEvent.Trigger(new RemoveMinerData(gameEvent.Miner));
        }
        
        private void OnUseBonus(UseBonusEvent gameEvent)
        {
            if (gameEvent.BoostType is BoostType.PowerAll or BoostType.Slots)
            {
                Relocate(_sessionData.Token);
            }
        }
        
        private void OnEndBonus(EndBonusEvent gameEvent)
        {
            if (gameEvent.BoostType is BoostType.PowerAll or BoostType.Slots)
            {
                Relocate(_sessionData.Token);
            }
        }

        public bool Drop(string minerId, int slot)
        {
            var playerMiners = _playerMinersRepository.Get(_sessionData.Token);
            var minerAtSlot = playerMiners.Miners[slot];
            
            var slotOfMiner = playerMiners.Miners.IndexOf(minerId);
            if (slotOfMiner == slot) return false;

            var miner1 = _minerRepository.Get(minerId);
            var miner2 = minerAtSlot != null ? _minerRepository.Get(minerAtSlot) : null;

            PlayerAction action = null;
            if (minerAtSlot == null || miner1.ConfigId != miner2.ConfigId)
            {
                _playerActionProxy.SwapMiners(slotOfMiner, slot);
                return true;
            }
            else
            {
                _playerActionProxy.MergeMiners(slotOfMiner, slot);
                return true;
            }
        }

        public void Remove(string minerId)
        {
            var slot = _playerMinersRepository.Get(_sessionData.Token).Miners.IndexOf(minerId);
            _playerActionProxy.RemoveMiner(slot);
        }
    }
    
    public struct RelocateData
    {
        public int Width { get; }
        public int Height { get; }
        public int Total => Width * Height;
        public int Powered { get; }
        
        public RelocateData(int width, int height, int powered)
        {
            Width = width;
            Height = height;
            Powered = powered;
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
        public int Level { get; }
        public int Slot { get; }
        public string[] Merged { get; }
        public bool MaxLevelIncreased { get; }

        public MergeMinersData(string id, int level, int slot, string[] merged, bool maxLevelIncreased)
        {
            Id = id;
            Level = level;
            Slot = slot;
            Merged = merged;
            MaxLevelIncreased = maxLevelIncreased;
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