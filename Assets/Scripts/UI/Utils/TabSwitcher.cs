using Utils;
using Utils.Reactive;

namespace UI.Utils
{
    public class TabSwitcher
    {
        private ReactiveEvent<Tab> _switchTabEvent = new();
        public IReactiveSubscription<Tab> SwitchTabEvent => _switchTabEvent;

        public void SwitchTab(Tab tab)
        {
            _switchTabEvent.Trigger(tab);
        }
    }

    public enum Tab
    {
        Game,
        Shop,
        Upgrades,
        Missions,
        Referral
    }
}