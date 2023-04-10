using System.Linq;
using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Services;
using Utils;

namespace _Proxy.Connectors
{
    public class PopupsConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly MinerConfig _minerConfig;
        private readonly RandomMinerService _randomMinerService;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<NewMinerPopupData> _newMinerPopupEvent = new();
        public IReactiveSubscription<NewMinerPopupData> NewMinerPopupEvent => _newMinerPopupEvent;
        
        private ReactiveEvent<RoulettePopupData> _roulettePopupEvent = new();
        public IReactiveSubscription<RoulettePopupData> RoulettePopupEvent => _roulettePopupEvent;

        public PopupsConnector(
            LocalPlayer localPlayer,
            MinerConfig minerConfig,
            RandomMinerService randomMinerService,
            EventSubscriptionService eventSubscriptionService)
        {
            _localPlayer = localPlayer;
            _minerConfig = minerConfig;
            _randomMinerService = randomMinerService;
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
        }

        public void RollRandom(string win)
        {
            var winConfig = _minerConfig.Get(win);
            var winData = new MinerData(win, winConfig.Level);
            _roulettePopupEvent.Trigger(new RoulettePopupData(winData, _randomMinerService.GetAvailableMiners(_localPlayer.Id).Select(x =>
            {
                var config = _minerConfig.Get(x);
                return new MinerData(x, config.Level);
            }).ToArray()));
        }
        
        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            var newMiner = _minerConfig.GetMiner(gameEvent.Level);
            var miner = _minerConfig.Get(newMiner).MoneyPerSecond;
            var previousMiner = _minerConfig.GetMiner(gameEvent.Level - 1);
            _newMinerPopupEvent.Trigger(new NewMinerPopupData(newMiner, previousMiner, gameEvent.Level, miner));
        }
    }

    public struct NewMinerPopupData
    {
        public string Config { get; }
        public string PreviousConfig { get; }
        public int Level { get; }
        public double Income { get; }

        public NewMinerPopupData(string config, string previousConfig, int level, double income)
        {
            Config = config;
            PreviousConfig = previousConfig;
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
        public string Config { get; }
        public int Level { get; }

        public MinerData(string config, int level)
        {
            Config = config;
            Level = level;
        }
    }
}