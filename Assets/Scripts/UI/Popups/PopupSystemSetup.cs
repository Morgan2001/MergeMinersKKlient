using _Proxy.Connectors;
using Common.Utils.Extensions;
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
        [SerializeField] private RoulettePopup _roulettePopup;
        [SerializeField] private RouletteWinPopup _rouletteWinPopup;
        [SerializeField] private RelocationPopup _relocationPopup;
        [SerializeField] private GiftPopup _giftPopup;

        private PopupsConnector _popupsConnector;
        private RelocateConnector _relocateConnector;
        private FreeGemConnector _freeGemConnector;
        private IMinerResourceHelper _resourceHelper;

        private IPopup _currentPopup;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            RelocateConnector relocateConnector,
            FreeGemConnector freeGemConnector, 
            IMinerResourceHelper resourceHelper)
        {
            _popupsConnector = popupsConnector;
            _relocateConnector = relocateConnector;
            _freeGemConnector = freeGemConnector;
            _resourceHelper = resourceHelper;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
            _popupsConnector.RoulettePopupEvent.Subscribe(OnRoulette);
            _popupsConnector.RelocationPopupEvent.Subscribe(OnRelocation);
            _popupsConnector.GiftPopupEvent.Subscribe(OnGift);
        }

        private void OnNewMiner(NewMinerPopupData data)
        {
            var icon = _resourceHelper.GetNormalIconByName(data.Config);
            var previousIcon = _resourceHelper.GetNormalIconByName(data.PreviousConfig);
            var viewModel = new NewMinerPopupViewModel(data.Config, data.Level, data.Income, icon, previousIcon);
            ShowPopup(_newMinerPopup, viewModel);
        }

        private void OnRoulette(RoulettePopupData data)
        {
            var icon = _resourceHelper.GetNormalIconByName(data.Config.Config);
            var win = new RouletteMinerInfo(data.Config.Level, icon);
            RouletteMinerInfo GetRandom()
            {
                var config = data.Variants.Random();
                var randomIcon = _resourceHelper.GetNormalIconByName(config.Config);
                return new RouletteMinerInfo(config.Level, randomIcon);
            }
            
            _roulettePopup.SpinEvent.Subscribe(() => OnRouletteWin(data.Config)).AddTo(_roulettePopup);
            
            var viewModel = new RoulettePopupViewModel(win, GetRandom);
            ShowPopup(_roulettePopup, viewModel);
        }

        private void OnRouletteWin(MinerData data)
        {
            var icon = _resourceHelper.GetNormalIconByName(data.Config);
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