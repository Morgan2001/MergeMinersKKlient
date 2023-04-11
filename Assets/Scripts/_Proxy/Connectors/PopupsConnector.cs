﻿using System;
using System.Linq;
using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using Utils;

namespace _Proxy.Connectors
{
    public class PopupsConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly GameConfig _gameConfig;
        private readonly MinerConfig _minerConfig;
        private readonly LocationConfig _locationConfig;
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerSlotsRepository _playerSlotsRepository;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly RandomMinerService _randomMinerService;
        private readonly FreeGemService _freeGemService;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<NewMinerPopupData> _newMinerPopupEvent = new();
        public IReactiveSubscription<NewMinerPopupData> NewMinerPopupEvent => _newMinerPopupEvent;
        
        private ReactiveEvent<RoulettePopupData> _roulettePopupEvent = new();
        public IReactiveSubscription<RoulettePopupData> RoulettePopupEvent => _roulettePopupEvent;
        
        private ReactiveEvent<RelocationPopupData> _relocationPopupEvent = new();
        public IReactiveSubscription<RelocationPopupData> RelocationPopupEvent => _relocationPopupEvent;

        private ReactiveEvent<GiftPopupData> _giftPopupEvent = new();
        public IReactiveSubscription<GiftPopupData> GiftPopupEvent => _giftPopupEvent;
        
        private ReactiveEvent<BonusPopupData> _bonusPopupEvent = new();
        public IReactiveSubscription<BonusPopupData> BonusPopupEvent => _bonusPopupEvent;
        
        public PopupsConnector(
            LocalPlayer localPlayer,
            GameConfig gameConfig,
            MinerConfig minerConfig,
            LocationConfig locationConfig,
            PlayerRepository playerRepository,
            PlayerSlotsRepository playerSlotsRepository,
            PlayerMinersRepository playerMinersRepository,
            RandomMinerService randomMinerService,
            FreeGemService freeGemService,
            EventSubscriptionService eventSubscriptionService)
        {
            _localPlayer = localPlayer;
            _gameConfig = gameConfig;
            _minerConfig = minerConfig;
            _locationConfig = locationConfig;
            _playerRepository = playerRepository;
            _playerSlotsRepository = playerSlotsRepository;
            _playerMinersRepository = playerMinersRepository;
            _randomMinerService = randomMinerService;
            _freeGemService = freeGemService;
            
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
        }

        public void RollRandom(int level)
        {
            var winData = new MinerData(level);
            _roulettePopupEvent.Trigger(new RoulettePopupData(winData, _randomMinerService.GetAvailableMiners(_localPlayer.Id)
                .Select(x => new MinerData(_minerConfig.Get(x).Level)).ToArray()));
        }

        public void ShowRelocation()
        {
            var player = _playerRepository.Get(_localPlayer.Id);
            var playerSlots = _playerSlotsRepository.Get(_localPlayer.Id);
            var playerMiners = _playerMinersRepository.Get(_localPlayer.Id);
            var location = _locationConfig.GetLocation(playerSlots.Level + 1);
            _relocationPopupEvent.Trigger(new RelocationPopupData(playerSlots.Level + 1, location.TotalSlots, location.PoweredSlots, location.MaxMinerLevel, location.MinerLevelRequired, playerMiners.MaxLevelAchieved, player.Money, location.Price));
        }
        
        public void ShowGift()
        {
            if (_freeGemService.GetGemProgress(_localPlayer.Id) < 1) return;
            _giftPopupEvent.Trigger(new GiftPopupData(_gameConfig.FreeGems));
        }
        
        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            var newMiner = _minerConfig.GetMiner(gameEvent.Level);
            var miner = _minerConfig.Get(newMiner).MoneyPerSecond;
            _newMinerPopupEvent.Trigger(new NewMinerPopupData(newMiner, gameEvent.Level, miner));
        }

        public void ShowBonus(BonusType bonusType, Action callback)
        {
            _bonusPopupEvent.Trigger(new BonusPopupData(bonusType, callback));
        }
    }

    public struct NewMinerPopupData
    {
        public string Config { get; }
        public int Level { get; }
        public double Income { get; }

        public NewMinerPopupData(string config, int level, double income)
        {
            Config = config;
            Level = level;
            Income = income;
        }
    }

    public struct RoulettePopupData
    {
        public MinerData Config { get; }
        public MinerData[] Variants { get; }

        public RoulettePopupData(MinerData config, MinerData[] variants)
        {
            Config = config;
            Variants = variants;
        }
    }

    public struct MinerData
    {
        public int Level { get; }

        public MinerData(int level)
        {
            Level = level;
        }
    }

    public struct RelocationPopupData
    {
        public int Level { get; }
        public int Slots { get; }
        public int Powered { get; }
        public int MaxMinerLevel { get; }
        public int MinMinerLevelNeeded { get; }
        public int CurrentMinerLevel { get; }
        public double CurrentMoney { get; }
        public double RelocateCost { get; }

        public RelocationPopupData(int level, int slots, int powered, int maxMinerLevel, int minMinerLevelNeeded, int currentMinerLevel, double currentMoney, double relocateCost)
        {
            Level = level;
            Slots = slots;
            Powered = powered;
            MaxMinerLevel = maxMinerLevel;
            MinMinerLevelNeeded = minMinerLevelNeeded;
            CurrentMinerLevel = currentMinerLevel;
            CurrentMoney = currentMoney;
            RelocateCost = relocateCost;
        }
    }

    public struct GiftPopupData
    {
        public int Gems { get; }

        public GiftPopupData(int gems)
        {
            Gems = gems;
        }
    }

    public class BonusPopupData
    {
        public BonusType BonusType { get; }
        public Action Callback { get; }

        public BonusPopupData(BonusType bonusType, Action callback)
        {
            BonusType = bonusType;
            Callback = callback;
        }
    }
}
