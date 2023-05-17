using GameCore.Connectors;
using I2.Loc;
using MergeMiner.Core.Network.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameCore.Preloader
{
    public class Preloader : MonoBehaviour
    {
        private SessionData _sessionData;
        private RestAPI _restAPI;
        private AlertConnector _alertConnector;
        
        [Inject]
        private void Setup(
            SessionData sessionData,
            RestAPI restAPI,
            AlertConnector alertConnector)
        {
            _sessionData = sessionData;
            _restAPI = restAPI;
            _alertConnector = alertConnector;
        }
        
        private async void Awake()
        {
            var status = await _restAPI.Status();
            if (!status)
            {
                var text = LocalizationManager.GetTranslation("alert_server");
                var label = LocalizationManager.GetTranslation("alert_button_exit");
                _alertConnector.ShowAlert(text, label, Application.Quit);
                return;
            }
            
            var version = await _restAPI.Version();
            if (version != Constants.Version)
            {
                var text = LocalizationManager.GetTranslation("alert_outdated");
                var label = LocalizationManager.GetTranslation("alert_upgrade");
                _alertConnector.ShowAlert(text, label, () => Application.OpenURL(Constants.GooglePlayUrl));
                return;
            }
            
            var data = await _restAPI.UserLogin(SystemInfo.deviceUniqueIdentifier);
            
            var configData = await _restAPI.GetConfig(data.Token);
            var config = ConfigHelper.GetConfig(configData);
            
            var gameState = await _restAPI.GetState(data.Token);
            
            _sessionData.Setup(data.Token, data.Email, config, gameState);
            SceneManager.LoadScene("Gameplay");
        }
    }
}