using _Proxy.Connectors;
using _Proxy.Data;
using Common.Utils.Extensions;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.Popups
{
    public class PopupSystemSetup : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        
        [SerializeField] private GameObject _shopScreen;
        [SerializeField] private GameObject _upgradesScreen;
        [SerializeField] private GameObject _mainScreen;
        
        [SerializeField] private NewMinerPopup _newMinerPopup;
        [SerializeField] private MinerRoulettePopup _minerRoulettePopup;
        [SerializeField] private RouletteWinPopup _rouletteWinPopup;
        [SerializeField] private RelocationPopup _relocationPopup;
        [SerializeField] private GiftPopup _giftPopup;
        [SerializeField] private WheelPopup _wheelPopup;
        [SerializeField] private WheelRewardPopup _wheelRewardPopup;
        
        [SerializeField] private BonusPopup _chipBonusPopup;
        [SerializeField] private BonusPopup _flashBonusPopup;
        [SerializeField] private BonusPopup _powerBonusPopup;
        [SerializeField] private BonusPopup _minersBonusPopup;

        private PopupsConnector _popupsConnector;
        private RelocateConnector _relocateConnector;
        private FreeGemConnector _freeGemConnector;
        private WheelConnector _wheelConnector;
        private IResourceHelper _resourceHelper;
        private TabSwitcher _tabSwitcher;

        private IPopup _currentPopup;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            RelocateConnector relocateConnector,
            FreeGemConnector freeGemConnector, 
            WheelConnector wheelConnector,
            IResourceHelper resourceHelper,
            TabSwitcher tabSwitcher)
        {
            _popupsConnector = popupsConnector;
            _relocateConnector = relocateConnector;
            _freeGemConnector = freeGemConnector;
            _wheelConnector = wheelConnector;
            _resourceHelper = resourceHelper;
            _tabSwitcher = tabSwitcher;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
            _popupsConnector.MinerRoulettePopupEvent.Subscribe(OnMinerRoulette);
            _popupsConnector.RelocationPopupEvent.Subscribe(OnRelocation);
            _popupsConnector.GiftPopupEvent.Subscribe(OnGift);
            _popupsConnector.BonusPopupEvent.Subscribe(OnBonus);
            _popupsConnector.WheelPopupEvent.Subscribe(OnWheel);
            _popupsConnector.WheelRewardPopupEvent.Subscribe(OnWheelReward);
            
            _tabSwitcher.SwitchTabEvent.Subscribe(OnSwitchTab);
        }
        
        private void Awake()
        {
            OnSwitchTab(Tab.Game);
        }
        
        private void OnSwitchTab(Tab tab)
        {
            _shopScreen.SetActive(tab == Tab.Shop);
            _upgradesScreen.SetActive(tab == Tab.Upgrades);
            _mainScreen.SetActive(tab == Tab.Game);
        }

        private void OnNewMiner(NewMinerPopupData data)
        {
            var icon = _resourceHelper.GetNormalIconByLevel(data.Level);
            var previousIcon = _resourceHelper.GetNormalIconByLevel(data.Level - 1);
            var viewModel = new NewMinerPopupViewModel(data.Config, data.Level, data.Income, icon, previousIcon);
            ShowPopup(_newMinerPopup, viewModel);
        }

        private void OnMinerRoulette(MinerRoulettePopupData data)
        {
            var icon = _resourceHelper.GetNormalIconByLevel(data.Config.Level);
            var win = new RouletteMinerInfo(data.Config.Level, icon);
            RouletteMinerInfo GetRandom()
            {
                var config = data.Variants.Random();
                var randomIcon = _resourceHelper.GetNormalIconByLevel(config.Level);
                return new RouletteMinerInfo(config.Level, randomIcon);
            }
            
            _minerRoulettePopup.SpinEvent.Subscribe(() => OnRouletteWin(data.Config)).AddTo(_minerRoulettePopup);
            
            var viewModel = new MinerRoulettePopupViewModel(win, GetRandom);
            ShowPopup(_minerRoulettePopup, viewModel);
        }

        private void OnRouletteWin(MinerData data)
        {
            var icon = _resourceHelper.GetNormalIconByLevel(data.Level);
            var viewModel = new RouletteWinPopupViewModel(data.Level, icon);
            ShowPopup(_rouletteWinPopup, viewModel);
        }
        
        private void OnRelocation(RelocationPopupData data)
        {
            var image = _resourceHelper.GetLocationImageByLevel(data.Level);
            var name = _resourceHelper.GetLocationNameByLevel(data.Level);
            
            _relocationPopup.ClickEvent.Subscribe(_relocateConnector.Relocate).AddTo(_relocationPopup);
            
            var viewModel = new RelocationPopupViewModel(image, name, data.Level, data.Slots, data.Powered, data.MaxMinerLevel, data.MinMinerLevelNeeded, data.CurrentMinerLevel, data.CurrentMoney, data.RelocateCost);
            ShowPopup(_relocationPopup, viewModel);
        }
        
        private void OnGift(GiftPopupData data)
        {
            _giftPopup.ClickEvent.Subscribe(_freeGemConnector.GetFreeGem).AddTo(_giftPopup);
            
            var viewModel = new GiftPopupViewModel(data.Gems);
            ShowPopup(_giftPopup, viewModel);
        }

        private void OnBonus(BonusPopupData data)
        {
            var viewModel = new BonusPopupViewModel();
            BonusPopup popup = null;
            switch (data.Id)
            {
                case BonusNames.Chip:
                {
                    popup = _chipBonusPopup;
                    break;
                }
                case BonusNames.Flash:
                {
                    popup = _flashBonusPopup;
                    break;
                }
                case BonusNames.Miners:
                {
                    popup = _minersBonusPopup;
                    break;
                }
                case BonusNames.Power:
                {
                    popup = _powerBonusPopup;
                    break;
                }
            }
            if (popup == null) return;
            
            popup.AcceptEvent.Subscribe(data.Callback).AddTo(popup);
            
            ShowPopup(popup, viewModel);
        }
        
        private void OnWheel()
        {
            var viewModel = new WheelPopupViewModel();
            ShowPopup(_wheelPopup, viewModel);
            
            _wheelConnector.SpinEvent.Subscribe(data => viewModel.Spin(data.Id)).AddTo(_wheelPopup);
            _wheelPopup.SpinEvent.Subscribe(_wheelConnector.Spin).AddTo(_wheelPopup);
            _wheelPopup.EndSpinEvent.Subscribe(_popupsConnector.ShowWheelReward).AddTo(_wheelPopup);
        }
        
        private void OnWheelReward(WheelRewardData data)
        {
            var viewModel = new WheelRewardPopupViewModel(data.Icon, data.Description);
            ShowPopup(_wheelRewardPopup, viewModel);
        }

        private void ShowPopup<T>(IPopup<T> popup, T data)
        {
            if (_currentPopup != null)
            {
                _currentPopup.HideForce();
                _currentPopup = null;
            }
            
            _currentPopup = popup;
            
            popup.CloseEvent.Subscribe(OnClosePopup).AddTo(popup);
            popup.Show(data);
            
            _container.SetActive(false);
        }

        private void OnClosePopup(IPopup popup)
        {
            _container.SetActive(true);
            _currentPopup = null;
        }
    }
}