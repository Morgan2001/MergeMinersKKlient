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
        
        private void Awake()
        {
            Connect();
        }

        private async void Connect()
        {
            var statusResult = await _restAPI.Status();
            if (statusResult.ResultType == RestResultType.Disconnected)
            {
                _alertConnector.ShowNoInternet(Connect);
                return;
            }
            
            if (statusResult.ResultType == RestResultType.Success)
            {
                if (!statusResult.Result)
                {
                    var text = LocalizationManager.GetTranslation("alert_server");
                    var label = LocalizationManager.GetTranslation("alert_button_exit");
                    _alertConnector.ShowAlert(text, label, Application.Quit);
                    return;
                }
            }
            
            var versionResult = await _restAPI.Version();
            if (versionResult.ResultType == RestResultType.Success)
            {
                if (versionResult.Result != Constants.Version)
                {
                    var text = LocalizationManager.GetTranslation("alert_outdated");
                    var label = LocalizationManager.GetTranslation("alert_upgrade");
                    _alertConnector.ShowAlert(text, label, () => Application.OpenURL(Constants.GooglePlayLink));
                    return;
                }
            }
            
            var userLoginResult = await _restAPI.UserLogin(SystemInfo.deviceUniqueIdentifier);
            if (userLoginResult.ResultType != RestResultType.Success) return;
            
            var token = userLoginResult.Result.Token;

            var configResult = await _restAPI.GetConfig(token);
            if (configResult.ResultType != RestResultType.Success) return;
            
            var config = ConfigHelper.GetConfig(configResult.Result);
            
            var gameStateResult = await _restAPI.GetState(token);
            if (gameStateResult.ResultType != RestResultType.Success) return;
            
            _sessionData.Setup(token, userLoginResult.Result.Email, config, gameStateResult.Result);
            SceneManager.LoadScene("Gameplay");
        }
    }
}