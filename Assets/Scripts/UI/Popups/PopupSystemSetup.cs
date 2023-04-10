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

        private PopupsConnector _popupsConnector;
        private IMinerResourceHelper _resourceHelper;

        private IPopup _currentPopup;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            IMinerResourceHelper resourceHelper)
        {
            _popupsConnector = popupsConnector;
            _resourceHelper = resourceHelper;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
            _popupsConnector.RoulettePopupEvent.Subscribe(OnRoulette);
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