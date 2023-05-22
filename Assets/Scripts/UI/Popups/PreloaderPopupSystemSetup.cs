using GameCore.Connectors;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.Popups
{
    public class PreloaderPopupSystemSetup : MonoBehaviour
    {
        [SerializeField] private AlertPopup _alertPopup;
        [SerializeField] private NoInternetPopup _noInternetPopup;

        private AlertConnector _alertConnector;

        private IPopup _currentPopup;

        private DisposableCarrier _carrier = new();

        [Inject]
        private void Setup(AlertConnector alertConnector)
        {
            _alertConnector = alertConnector;
            _alertConnector.AlertPopupEvent.Subscribe(OnAlert).AddTo(_carrier);
            _alertConnector.NoInternetPopupEvent.Subscribe(OnNoInternet).AddTo(_carrier);
        }

        private void OnDestroy()
        {
            _carrier.Dispose();
        }

        private void OnAlert(AlertData data)
        {
            var viewModel = new AlertPopupViewModel(data.Text, data.ButtonLabel, data.ButtonAction);
            ShowPopup(_alertPopup, viewModel);
        }
        
        private void OnNoInternet(NoInternetData data)
        {
            var viewModel = new NoInternetPopupViewModel(data.ReconnectAction);
            ShowPopup(_noInternetPopup, viewModel);
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
        }

        private void OnClosePopup(IPopup popup)
        {
            _currentPopup = null;
        }
    }
}