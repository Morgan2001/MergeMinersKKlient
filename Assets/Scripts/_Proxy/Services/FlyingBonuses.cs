using _Proxy.Data;
using _Proxy.Events;
using _Proxy.Preloader;
using Common.Utils.Misc;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Services;

namespace _Proxy.Services
{
    public class FlyingBonuses
    {
        private readonly SessionData _sessionData;
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly TimerService _timerService;
        private readonly RandomExecutor _randomExecutor;

        private float _delay;

        public FlyingBonuses(
            SessionData sessionData,
            EventDispatcherService eventDispatcherService,
            TimerService timerService,
            RandomExecutor randomExecutor)
        {
            _sessionData = sessionData;
            _eventDispatcherService = eventDispatcherService;
            _randomExecutor = randomExecutor;
            
            _timerService = timerService;
            _timerService.TickDeltaEvent.Subscribe(OnTick);
        }

        public void Reset()
        {
            _delay = 20f;
        }

        private void OnTick(float deltaTime)
        {
            _delay -= deltaTime;
            if (_delay > 0) return;
            
            var bonus = _randomExecutor.Random<BonusType>();
            _eventDispatcherService.Dispatch(new AddBonusEvent(_sessionData.Token, bonus));
            Reset();
        }
    }
}