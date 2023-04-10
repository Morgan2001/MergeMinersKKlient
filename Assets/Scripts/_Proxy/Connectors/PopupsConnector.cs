using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.State.Config;
using Utils;

namespace _Proxy.Connectors
{
    public class PopupsConnector
    {
        private readonly MinerConfig _minerConfig;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<NewMinerPopupData> _newMinerPopupEvent = new();
        public IReactiveSubscription<NewMinerPopupData> NewMinerPopupEvent => _newMinerPopupEvent;

        public PopupsConnector(
            MinerConfig minerConfig,
            EventSubscriptionService eventSubscriptionService)
        {
            _minerConfig = minerConfig;
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
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
}