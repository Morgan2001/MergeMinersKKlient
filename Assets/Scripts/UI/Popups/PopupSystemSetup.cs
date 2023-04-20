using _Proxy.Connectors;
using Common.Utils.Extensions;
using MergeMiner.Core.State.Config;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.Popups
{
    public class PopupSystemSetup : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainScreen;
        
        [SerializeField] private NewMinerPopup _newMinerPopup;
        [SerializeField] private MinerRoulettePopup _minerRoulettePopup;
        [SerializeField] private RouletteWinPopup _rouletteWinPopup;
        [SerializeField] private RelocationPopup _relocationPopup;
        [SerializeField] private GiftPopup _giftPopup;
        
        [SerializeField] private BonusPopup _chipBonusPopup;
        [SerializeField] private BonusPopup _flashBonusPopup;
        [SerializeField] private BonusPopup _powerBonusPopup;
        [SerializeField] private BonusPopup _minersBonusPopup;
        
        // [SerializeField] private MinerRoulettePopup _roulettePopup;

        private PopupsConnector _popupsConnector;
        private RelocateConnector _relocateConnector;
        private FreeGemConnector _freeGemConnector;
        private IResourceHelper _resourceHelper;

        private IPopup _currentPopup;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            RelocateConnector relocateConnector,
            FreeGemConnector freeGemConnector, 
            IResourceHelper resourceHelper)
        {
            _popupsConnector = popupsConnector;
            _relocateConnector = relocateConnector;
            _freeGemConnector = freeGemConnector;
            _resourceHelper = resourceHelper;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
            _popupsConnector.MinerRoulettePopupEvent.Subscribe(OnMinerRoulette);
            _popupsConnector.RelocationPopupEvent.Subscribe(OnRelocation);
            _popupsConnector.GiftPopupEvent.Subscribe(OnGift);
            _popupsConnector.BonusPopupEvent.Subscribe(OnBonus);
            // _popupsConnector.RoulettePopupEvent.Subscribe(OnRoulette);
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
            switch (data.BonusType)
            {
                case BonusType.Chip:
                {
                    popup = _chipBonusPopup;
                    break;
                }
                case BonusType.Flash:
                {
                    popup = _flashBonusPopup;
                    break;
                }
                case BonusType.Miners:
                {
                    popup = _minersBonusPopup;
                    break;
                }
                case BonusType.Power:
                {
                    popup = _powerBonusPopup;
                    break;
                }
            }
            if (popup == null) return;
            
            popup.AcceptEvent.Subscribe(data.Callback).AddTo(popup);
            
            ShowPopup(popup, viewModel);
        }
        
        // private void OnRoulette(NewMinerPopupData data)
        // {
            // var viewModel = new NewMinerPopupViewModel(data.Config, data.Level, data.Income, icon, previousIcon);
            // ShowPopup(_ro, viewModel);
        // }

        private void ShowPopup<T>(IPopup<T> popup, T data)
        {
            if (_currentPopup != null)
            {
                _currentPopup.Hide();
            }
            
            _currentPopup = popup;
            _currentPopup.CloseEvent.Subscribe(OnClosePopup);
            
            popup.Show(data);
            
            _mainScreen.gameObject.SetActive(false);
        }

        private void OnClosePopup(IPopup popup)
        {
            popup.CloseEvent.Unsubscribe(OnClosePopup);

            if (_currentPopup != popup) return;
            
            _mainScreen.gameObject.SetActive(true);
            _currentPopup = null;
        }
    }
}