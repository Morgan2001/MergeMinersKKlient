using GameCore.Connectors;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;
using Zenject;

namespace UI.TopPanel
{
    public class TopPanelSetup : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private ResourcesView _resourcesView;
        [SerializeField] private FilledButtonView _freeGemView;
        [SerializeField] private FilledButtonView _relocateView;
        [SerializeField] private FilledButtonView _balanceView;
        
        private PlayerConnector _playerConnector;
        private FreeGemConnector _freeGemConnector;
        private RelocateConnector _relocateConnector;
        private BalanceConnector _balanceConnector;
        private PopupsConnector _popupsConnector;
        private TabSwitcher _tabSwitcher;

        private ResourcesViewModel _resourcesViewModel;
        private FilledButtonViewModel _freeGemViewModel;
        private FilledButtonViewModel _relocateViewModel;
        private FilledButtonViewModel _balanceViewModel;

        [Inject]
        private void Setup(
            PlayerConnector playerConnector, 
            FreeGemConnector freeGemConnector, 
            RelocateConnector relocateConnector,
            BalanceConnector balanceConnector,
            PopupsConnector popupsConnector,
            TabSwitcher tabSwitcher)
        {
            _playerConnector = playerConnector;
            _freeGemConnector = freeGemConnector;
            _relocateConnector = relocateConnector;
            _balanceConnector = balanceConnector;
            _popupsConnector = popupsConnector;
            _tabSwitcher = tabSwitcher;

            SetupResources();
            SetupFreeGems();
            SetupRelocator();
            SetupBalance();

            _settingsButton.Subscribe(_popupsConnector.ShowEmail);
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
            _freeGemViewModel = new FilledButtonViewModel();
            _freeGemView.Bind(_freeGemViewModel);
            
            _freeGemConnector.Progress.Subscribe(_freeGemViewModel.SetProgress).AddTo(_freeGemView);
            _freeGemViewModel.ButtonClickEvent.Subscribe(_popupsConnector.ShowGift).AddTo(_freeGemView);
        }

        private void SetupRelocator()
        {
            _relocateViewModel = new FilledButtonViewModel();
            _relocateView.Bind(_relocateViewModel);
            
            _relocateConnector.Progress.Bind(_relocateViewModel.SetProgress).AddTo(_relocateViewModel);
            _relocateViewModel.ButtonClickEvent.Subscribe(_popupsConnector.ShowRelocation).AddTo(_relocateViewModel);
        }
        
        private void SetupBalance()
        {
            _balanceViewModel = new FilledButtonViewModel();
            _balanceView.Bind(_balanceViewModel);
            
            _balanceConnector.Progress.Bind(_balanceViewModel.SetProgress).AddTo(_balanceViewModel);
            _balanceViewModel.ButtonClickEvent.Subscribe(_popupsConnector.ShowBalance).AddTo(_balanceViewModel);
        }
    }
}