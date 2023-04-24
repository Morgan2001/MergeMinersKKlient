using Common.Utils.Events;

namespace GameCore.Services
{
    public class TimerService
    {
        private GameEventHandler _tickEvent = new();
        public IEventListener TickEvent => _tickEvent;
        
        private GameEventHandler<float> _tickDeltaEvent = new();
        public IEventListener<float> TickDeltaEvent => _tickDeltaEvent;
        
        public void Tick(float deltaTime)
        {
            _tickEvent.Trigger();
            _tickDeltaEvent.Trigger(deltaTime);
        }
    }
}