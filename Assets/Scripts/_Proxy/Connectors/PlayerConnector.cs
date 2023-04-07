using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using Utils;

namespace _Proxy.Connectors
{
    public class PlayerConnector
    {
        private readonly EventSubscriptionService _eventSubscriptionService;
        private ReactiveEvent<double> _moneyUpdateEvent = new();
        public IReactiveSubscription<double> MoneyUpdateEvent => _moneyUpdateEvent;
        
        private ReactiveEvent<int> _gemsUpdateEvent = new();
        public IReactiveSubscription<int> GemsUpdateEvent => _gemsUpdateEvent;
        
        public PlayerConnector(
            EventSubscriptionService eventSubscriptionService)
        {
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<ChangeMoneyEvent>(OnChangeMoney);
            _eventSubscriptionService.Subscribe<ChangeGemsEvent>(OnChangeGems);
        }

        private void OnChangeMoney(ChangeMoneyEvent gameEvent)
        {
            _moneyUpdateEvent.Trigger(gameEvent.Money);
        }
        
        private void OnChangeGems(ChangeGemsEvent gameEvent)
        {
            _gemsUpdateEvent.Trigger(gameEvent.Gems);
        }
    }
}