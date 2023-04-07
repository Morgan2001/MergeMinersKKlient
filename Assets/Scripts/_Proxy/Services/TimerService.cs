using Common.Utils.Events;

namespace _Proxy.Services
{
    public class TimerService
    {
        private EventHandler _tickEvent = new();
        public IEventListener TickEvent => _tickEvent;
        
        public void Tick()
        {
            _tickEvent.Trigger();
        }
    }
}