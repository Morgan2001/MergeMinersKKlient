using GameCore.Connectors;
using GameCore.Data;
using Common.Utils.Extensions;
using I2.Loc;
using MergeMiner.Core.State.Data;
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
        [SerializeField] private GameObject _missionsScreen;
        [SerializeField] private GameObject _referralScreen;
        
        [SerializeField] private NewMinerPopup _newMinerPopup;
        [SerializeField] private MinerRoulettePopup _minerRoulettePopup;
        [SerializeField] private RouletteWinPopup _rouletteWinPopup;
        [SerializeField] private RelocationPopup _relocationPopup;
        [SerializeField] private GiftPopup _giftPopup;
        [SerializeField] private WheelPopup _wheelPopup;
        [SerializeField] private WheelRewardPopup _wheelRewardPopup;
        [SerializeField] private OfflineIncomePopup _offlineIncomePopup;
        [SerializeField] private EmailPopup _emailPopup;
        [SerializeField] private BalancePopup _balancePopup;
        [SerializeField] private AlertPopup _alertPopup;
        
        [SerializeField] private BonusPopup _chipBonusPopup;
        [SerializeField] private BonusPopup _flashBonusPopup;
        [SerializeField] private BonusPopup _powerBonusPopup;
        [SerializeField] private BonusPopup _minersBonusPopup;

        private PopupsConnector _popupsConnector;
        private RelocateConnector _relocateConnector;
        private FreeGemConnector _freeGemConnector;
        private WheelConnector _wheelConnector;
        private AdsConnector _adsConnector;
        private OfflineIncomeConnector _offlineIncomeConnector;
        private EmailConnector _emailConnector;
        private IResourceHelper _resourceHelper;
        private TabSwitcher _tabSwitcher;

        private IPopup _currentPopup;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            RelocateConnector relocateConnector,
            FreeGemConnector freeGemConnector, 
            WheelConnector wheelConnector,
            AdsConnector adsConnector,
            OfflineIncomeConnector offlineIncomeConnector,
            EmailConnector emailConnector,
            IResourceHelper resourceHelper,
            TabSwitcher tabSwitcher)
        {
            _popupsConnector = popupsConnector;
            _relocateConnector = relocateConnector;
            _freeGemConnector = freeGemConnector;
            _wheelConnector = wheelConnector;
            _adsConnector = adsConnector;
            _offlineIncomeConnector = offlineIncomeConnector;
            _emailConnector = emailConnector;
            _resourceHelper = resourceHelper;
            _tabSwitcher = tabSwitcher;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
            _popupsConnector.MinerRoulettePopupEvent.Subscribe(OnMinerRoulette);
            _popupsConnector.RelocationPopupEvent.Subscribe(OnRelocation);
            _popupsConnector.GiftPopupEvent.Subscribe(OnGift);
            _popupsConnector.BonusPopupEvent.Subscribe(OnBonus);
            _popupsConnector.WheelPopupEvent.Subscribe(OnWheel);
            _popupsConnector.WheelRewardPopupEvent.Subscribe(OnWheelReward);
            _popupsConnector.OfflineIncomePopupEvent.Subscribe(OnOfflineIncome);
            _popupsConnector.EmailPopupEvent.Subscribe(OnEmail);
            _popupsConnector.BalancePopupEvent.Subscribe(OnBalance);
            _popupsConnector.AlertPopupEvent.Subscribe(OnAlert);
            
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
            _missionsScreen.SetActive(tab == Tab.Missions);
            _referralScreen.SetActive(tab == Tab.Referral);
        }

        private void OnNewMiner(NewMinerPopupData data)
        {
            var previousMiner = (data.PreviousMiner.Item1, data.PreviousMiner.Item2, data.PreviousMiner.Item3, _resourceHelper.GetNormalIconByLevel(data.PreviousMiner.Item2));
            var newMiner = (data.NewMiner.Item1, data.NewMiner.Item2, data.NewMiner.Item3, _resourceHelper.GetNormalIconByLevel(data.NewMiner.Item2));
            var viewModel = new NewMinerPopupViewModel(previousMiner, newMiner);
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
            var name = LocalizationManager.GetTranslation("location_" + data.Level);
            
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
            _wheelPopup.SpinEvent.Subscribe(Spin).AddTo(_wheelPopup);
            _wheelPopup.EndSpinEvent.Subscribe(_popupsConnector.ShowWheelReward).AddTo(_wheelPopup);
        }

        private void Spin(Currency currency)
        {
            if (currency == Currency.Ads)
            {
                _adsConnector.ShowRewarded(x =>
                {
                    if (x)
                    {
                        _wheelConnector.Spin(currency);
                    }
                });
            }
            else
            {
                _wheelConnector.Spin(currency);
            }
        }

        private void OnWheelReward(WheelRewardData data)
        {
            var viewModel = new WheelRewardPopupViewModel(data.Icon, data.Description);
            ShowPopup(_wheelRewardPopup, viewModel);
        }
        
        private void OnOfflineIncome(OfflineIncomeData data)
        {
            var viewModel = new OfflineIncomePopupViewModel(data.Income, data.MultipliedIncome);
            ShowPopup(_offlineIncomePopup, viewModel);

            _offlineIncomePopup.ClickEvent.Subscribe(MultiplyIncome).AddTo(_offlineIncomePopup);
        }
        
        private void OnEmail()
        {
            var viewModel = new EmailPopupViewModel();
            ShowPopup(_emailPopup, viewModel);
            
            _emailPopup.RegistrationEvent.Subscribe(_emailConnector.Register).AddTo(_emailPopup);
            _emailPopup.ForgetEvent.Subscribe(_emailConnector.Forget).AddTo(_emailPopup);
            _emailPopup.LoginEvent.Subscribe(_emailConnector.Login).AddTo(_emailPopup);
        }
        
        private void OnBalance(BalanceData data)
        {
            var viewModel = new BalancePopupViewModel(data.Gems, data.Token);
            ShowPopup(_balancePopup, viewModel);
        }
        
        private void OnAlert(AlertData data)
        {
            var viewModel = new AlertPopupViewModel(data.Text, data.ButtonLabel, data.ButtonAction);
            ShowPopup(_alertPopup, viewModel);
        }

        private void MultiplyIncome(bool value)
        {
            if (value)
            {
                _adsConnector.ShowRewarded(_offlineIncomeConnector.MultiplyIncome);
            }
            else
            {
                _offlineIncomeConnector.MultiplyIncome(false);
            }
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