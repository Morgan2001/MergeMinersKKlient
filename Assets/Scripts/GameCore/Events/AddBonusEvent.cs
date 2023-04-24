using MergeMiner.Core.State.Events.Base;

namespace GameCore.Events
{
    public class AddBonusEvent : GameEvent
    {
        public string Id { get; }

        public AddBonusEvent(string player, string id) : base(player)
        {
            Id = id;
        }
    }
}