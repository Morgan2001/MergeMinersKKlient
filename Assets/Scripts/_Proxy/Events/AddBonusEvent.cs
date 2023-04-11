using MergeMiner.Core.Events.Base;
using MergeMiner.Core.State.Config;

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