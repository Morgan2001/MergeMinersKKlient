using System;
using System.Linq;
using _Proxy.Events;
using _Proxy.Preloader;
using Common.Utils.Misc;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using UI.Utils;
using UnityEngine;
using Utils;

namespace _Proxy.Connectors
{
    public class PopupsConnector
    {
        private readonly SessionData _sessionData;
        private readonly GameConfig _gameConfig;
        private readonly MinerConfig _minerConfig;
        private readonly LocationConfig _locationConfig;
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerSlotsRepository _playerSlotsRepository;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly RandomMinerService _randomMinerService;
        private readonly FreeGemService _freeGemService;
        private readonly IncomeService _incomeService;
        private readonly TimeService _timeService;
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly IResourceHelper _resourceHelper;

        private ReactiveEvent<NewMinerPopupData> _newMinerPopupEvent = new();
        public IReactiveSubscription<NewMinerPopupData> NewMinerPopupEvent => _newMinerPopupEvent;
        
        private ReactiveEvent<MinerRoulettePopupData> _minerRoulettePopupEvent = new();
        public IReactiveSubscription<MinerRoulettePopupData> MinerRoulettePopupEvent => _minerRoulettePopupEvent;
        
        private ReactiveEvent<RelocationPopupData> _relocationPopupEvent = new();
        public IReactiveSubscription<RelocationPopupData> RelocationPopupEvent => _relocationPopupEvent;

        private ReactiveEvent<GiftPopupData> _giftPopupEvent = new();
        public IReactiveSubscription<GiftPopupData> GiftPopupEvent => _giftPopupEvent;
        
        private ReactiveEvent<BonusPopupData> _bonusPopupEvent = new();
        public IReactiveSubscription<BonusPopupData> BonusPopupEvent => _bonusPopupEvent;
        
        private ReactiveEvent _wheelPopupEvent = new();
        public IReactiveSubscription WheelPopupEvent => _wheelPopupEvent;
        
        private ReactiveEvent<WheelRewardData> _wheelRewardPopupEvent = new();
        public IReactiveSubscription<WheelRewardData> WheelRewardPopupEvent => _wheelRewardPopupEvent;
        
        private ReactiveEvent<OfflineIncomeData> _offlineIncomePopupEvent = new();
        public IReactiveSubscription<OfflineIncomeData> OfflineIncomePopupEvent => _offlineIncomePopupEvent;
        
        public PopupsConnector(
            SessionData sessionData,
            GameConfig gameConfig,
            MinerConfig minerConfig,
            LocationConfig locationConfig,
            PlayerRepository playerRepository,
            PlayerSlotsRepository playerSlotsRepository,
            PlayerMinersRepository playerMinersRepository,
            RandomMinerService randomMinerService,
            FreeGemService freeGemService,
            IncomeService incomeService,
            TimeService timeService,
            EventSubscriptionService eventSubscriptionService,
            IResourceHelper resourceHelper)
        {
            _sessionData = sessionData;
            _gameConfig = gameConfig;
            _minerConfig = minerConfig;
            _locationConfig = locationConfig;
            _playerRepository = playerRepository;
            _playerSlotsRepository = playerSlotsRepository;
            _playerMinersRepository = playerMinersRepository;
            _randomMinerService = randomMinerService;
            _freeGemService = freeGemService;
            _incomeService = incomeService;
            _timeService = timeService;
            _resourceHelper = resourceHelper;
            
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInit);
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
        }

        private void OnInit(InitGameEvent gameEvent)
        {
            var player = _playerRepository.Get(_sessionData.Token);
            var income = _incomeService.CalculateIncome(_sessionData.Token, player.LastUpdate, _timeService.GetCurrentTime);
            var multiplier = _gameConfig.OfflineIncomeMultiplier;
            _offlineIncomePopupEvent.Trigger(new OfflineIncomeData(income, income * multiplier));
        }

        public void RollRandom(int level)
        {
            var winData = new MinerData(level);
            _minerRoulettePopupEvent.Trigger(new MinerRoulettePopupData(winData, _randomMinerService.GetAvailableMiners(_sessionData.Token)
                .Select(x => new MinerData(_minerConfig.Get(x).Level)).ToArray()));
        }

        public void ShowRelocation()
        {
            var player = _playerRepository.Get(_sessionData.Token);
            var playerSlots = _playerSlotsRepository.Get(_sessionData.Token);
            var playerMiners = _playerMinersRepository.Get(_sessionData.Token);
            var location = _locationConfig.GetLocation(playerSlots.Level + 1);
            _relocationPopupEvent.Trigger(new RelocationPopupData(playerSlots.Level + 1, location.TotalSlots, location.PoweredSlots, location.MaxMinerLevel, location.MinerLevelRequired, playerMiners.MaxLevelAchieved, player.Money, location.Price));
        }
        
        public void ShowGift()
        {
            if (_freeGemService.GetGemProgress(_sessionData.Token) < 1) return;
            _giftPopupEvent.Trigger(new GiftPopupData(_gameConfig.FreeGems));
        }
        
        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            var newMiner = _minerConfig.GetMiner(gameEvent.Level);
            var miner = _minerConfig.Get(newMiner).MoneyPerSecond;
            _newMinerPopupEvent.Trigger(new NewMinerPopupData(newMiner, gameEvent.Level, miner));
        }

        public void ShowBonus(string id, Action callback)
        {
            _bonusPopupEvent.Trigger(new BonusPopupData(id, callback));
        }

        public void SpinWheel()
        {
            _wheelPopupEvent.Trigger();
        }

        public void ShowWheelReward(int reward)
        {
            var icon = _resourceHelper.GetWheelRewardIcon(reward);
            var description = _resourceHelper.GetWheelRewardDescription(reward);
            _wheelRewardPopupEvent.Trigger(new WheelRewardData(icon, description));
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

    public struct MinerRoulettePopupData
    {
        public MinerData Config { get; }
        public MinerData[] Variants { get; }

        public MinerRoulettePopupData(MinerData config, MinerData[] variants)
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
        public string Id { get; }
        public Action Callback { get; }

        public BonusPopupData(string id, Action callback)
        {
            Id = id;
            Callback = callback;
        }
    }

    public class WheelRewardData
    {
        public Sprite Icon { get; }
        public string Description { get; }

        public WheelRewardData(Sprite icon, string description)
        {
            Icon = icon;
            Description = description;
        }
    }
    
    public class OfflineIncomeData
    {
        public double Income { get; }
        public double MultipliedIncome { get; }

        public OfflineIncomeData(double income, double multipliedIncome)
        {
            Income = income;
            MultipliedIncome = multipliedIncome;
        }
    }
}
