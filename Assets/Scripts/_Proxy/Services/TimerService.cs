using Common.Utils.Events;

namespace _Proxy.Services
{
    public class TimerService
    {
        private EventHandler _tickEvent = new();
        public IEventListener TickEvent => _tickEvent;
        
        private EventHandler<float> _tickDeltaEvent = new();
        public IEventListener<float> TickDeltaEvent => _tickDeltaEvent;
        
        public void Tick(float deltaTime)
        {
            _tickEvent.Trigger();
            _tickDeltaEvent.Trigger(deltaTime);
        }
    }
}