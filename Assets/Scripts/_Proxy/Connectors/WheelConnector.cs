using System.Linq;
using _Proxy.Events;
using _Proxy.Preloader;
using _Proxy.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Services;
using ModestTree;
using Utils;

namespace _Proxy.Connectors
{
    public class WheelConnector
    {
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly WheelConfig _wheelConfig;

        private ReactiveEvent<SpinData> _spinEvent = new();
        public IReactiveSubscription<SpinData> SpinEvent => _spinEvent;

        public WheelConnector(
            PlayerActionProxy playerActionProxy,
            EventSubscriptionService eventSubscriptionService,
            WheelConfig wheelConfig)
        {
            _playerActionProxy = playerActionProxy;
            _wheelConfig = wheelConfig;
            
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<SpinWheelEvent>(OnSpinWheel);
        }

        public void Spin(Currency currency)
        {
            _playerActionProxy.SpinWheel(currency);
        }

        private void OnSpinWheel(SpinWheelEvent gameEvent)
        {
            var wheelItem = _wheelConfig.Get(gameEvent.Id);
            var id = _wheelConfig.GetAll().ToArray().IndexOf(wheelItem);
            _spinEvent.Trigger(new SpinData(id));
        }
    }

    public class SpinData
    {
        public int Id { get; }

        public SpinData(int id)
        {
            Id = id;
        }
    }
}