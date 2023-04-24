using _Proxy.Connectors;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.TopPanel
{
    public class TopPanelSetup : MonoBehaviour
    {
        [SerializeField] private ResourcesView _resourcesView;
        [SerializeField] private FreeGemView _freeGemView;
        [SerializeField] private RelocateView _relocateView;
        
        private PlayerConnector _playerConnector;
        private FreeGemConnector _freeGemConnector;
        private RelocateConnector _relocateConnector;
        private PopupsConnector _popupsConnector;
        private TabSwitcher _tabSwitcher;

        private ResourcesViewModel _resourcesViewModel;
        private FreeGemViewModel _freeGemViewModel;
        private RelocateViewModel _relocateViewModel;

        [Inject]
        private void Setup(
            PlayerConnector playerConnector, 
            FreeGemConnector freeGemConnector, 
            RelocateConnector relocateConnector,
            PopupsConnector popupsConnector,
            TabSwitcher tabSwitcher)
        {
            _playerConnector = playerConnector;
            _freeGemConnector = freeGemConnector;
            _relocateConnector = relocateConnector;
            _popupsConnector = popupsConnector;
            _tabSwitcher = tabSwitcher;

            SetupResources();
            SetupFreeGems();
            SetupRelocator();
        }

        private void SetupResources()
        {
            _resourcesViewModel = new ResourcesViewModel();
            _resourcesView.Bind(_resourcesViewModel);
            
            _playerConnector.MoneyUpdateEvent.Subscribe(_resourcesViewModel.SetMoney).AddTo(_resourcesView);
            _playerConnector.GemsUpdateEvent.Subscribe(_resourcesViewModel.SetGems).AddTo(_resourcesView);

            _resourcesView.ShopEvent.Subscribe(() => _tabSwitcher.SwitchTab(Tab.Shop)).AddTo(_relocateView);
        }

        private void SetupFreeGems()
        {
            _freeGemViewModel = new FreeGemViewModel();
            _freeGemView.Bind(_freeGemViewModel);
            
            _freeGemConnector.Progress.Subscribe(_freeGemViewModel.SetProgress).AddTo(_freeGemView);
            _freeGemViewModel.ButtonClickEvent.Subscribe(_popupsConnector.ShowGift).AddTo(_freeGemView);
        }

        private void SetupRelocator()
        {
            _relocateViewModel = new RelocateViewModel();
            _relocateView.Bind(_relocateViewModel);
            
            _relocateConnector.Progress.Bind(_relocateViewModel.SetProgress).AddTo(_relocateViewModel);
            _relocateViewModel.ButtonClickEvent.Subscribe(_popupsConnector.ShowRelocation).AddTo(_relocateViewModel);
        }
    }
}