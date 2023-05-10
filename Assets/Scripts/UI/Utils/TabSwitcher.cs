using GameCore.Connectors;
using GameCore.Preloader;
using Utils.Reactive;
using Zenject;

namespace UI.Utils
{
    public class TabSwitcher
    {
        private ReactiveEvent<Tab> _switchTabEvent = new();
        public IReactiveSubscription<Tab> SwitchTabEvent => _switchTabEvent;

        private SessionData _sessionData;
        private AdsConnector _adsConnector;

        private Tab _currentTab = Tab.Game;

        [Inject]
        private void Setup(
            SessionData sessionData,
            AdsConnector adsConnector)
        {
            _sessionData = sessionData;
            _adsConnector = adsConnector;
        }

        public void SwitchTab(Tab tab)
        {
            if (_currentTab == tab) return;
            
            _sessionData.SetWorking(tab == Tab.Game);
            
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