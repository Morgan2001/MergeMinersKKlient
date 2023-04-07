using _Proxy.Connectors;
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

        private ResourcesViewModel _resourcesViewModel;
        private FreeGemViewModel _freeGemViewModel;
        private RelocateViewModel _relocateViewModel;

        [Inject]
        private void Setup(PlayerConnector playerConnector, FreeGemConnector freeGemConnector, RelocateConnector relocateConnector)
        {
            _playerConnector = playerConnector;
            SetupResources();

            _freeGemConnector = freeGemConnector;
            SetupFreeGems();

            _relocateConnector = relocateConnector;
            SetupRelocator();
        }

        private void SetupResources()
        {
            _resourcesViewModel = new ResourcesViewModel();
            _resourcesView.Bind(_resourcesViewModel);
            
            _playerConnector.MoneyUpdateEvent.Subscribe(_resourcesViewModel.SetMoney).AddTo(_resourcesView);
            _playerConnector.GemsUpdateEvent.Subscribe(_resourcesViewModel.SetGems).AddTo(_resourcesView);
        }

        private void SetupFreeGems()
        {
            _freeGemViewModel = new FreeGemViewModel();
            _freeGemView.Bind(_freeGemViewModel);
            
            _freeGemConnector.Progress.Subscribe(_freeGemViewModel.SetProgress).AddTo(_freeGemView);
            _freeGemViewModel.ButtonClickEvent.Subscribe(_freeGemConnector.GetFreeGem).AddTo(_freeGemView);
        }

        private void SetupRelocator()
        {
            _relocateViewModel = new RelocateViewModel();
            _relocateView.Bind(_relocateViewModel);
            
            _relocateConnector.Progress.Bind(_relocateViewModel.SetProgress).AddTo(_relocateViewModel);
            _relocateViewModel.ButtonClickEvent.Subscribe(_relocateConnector.Relocate).AddTo(_relocateViewModel);
        }
    }
}