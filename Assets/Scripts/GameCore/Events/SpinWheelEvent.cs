using MergeMiner.Core.State.Events.Base;

namespace GameCore.Events
{
    public class SpinWheelEvent : GameEvent
    {
        public string Id { get; }
        
        public SpinWheelEvent(string player, string id) : base(player)
        {
            Id = id;
        }
    }
}