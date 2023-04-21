using MergeMiner.Core.State.Events.Base;

namespace _Proxy.Events
{
    public class InitGameEvent : GameEvent
    {
        public bool Offline { get; }
        
        public InitGameEvent(string player, bool offline) : base(player)
        {
            Offline = offline;
        }
    }
}