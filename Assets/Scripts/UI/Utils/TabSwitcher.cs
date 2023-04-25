using GameCore.Connectors;
using Utils.Reactive;
using Zenject;

namespace UI.Utils
{
    public class TabSwitcher
    {
        private ReactiveEvent<Tab> _switchTabEvent = new();
        public IReactiveSubscription<Tab> SwitchTabEvent => _switchTabEvent;

        private Tab _currentTab = Tab.Game;
        private AdsConnector _adsConnector;

        [Inject]
        private void Setup(
            AdsConnector adsConnector)
        {
            _adsConnector = adsConnector;
        }

        public void SwitchTab(Tab tab)
        {
            if (_currentTab == tab) return;
            _switchTabEvent.Trigger(tab);
            _adsConnector.ShowInterstitial();
            _currentTab = tab;
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