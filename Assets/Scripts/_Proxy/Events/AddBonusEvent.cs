using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events.Base;

namespace _Proxy.Events
{
    public class AddBonusEvent : GameEvent
    {
        public BonusType BonusType { get; }
        
        public AddBonusEvent(string player, BonusType bonusType) : base(player)
        {
            BonusType = bonusType;
        }
    }
}