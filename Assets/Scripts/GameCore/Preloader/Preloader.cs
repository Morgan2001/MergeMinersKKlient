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
        
        [Inject]
        private void Setup(
            SessionData sessionData,
            RestAPI restAPI)
        {
            _sessionData = sessionData;
            _restAPI = restAPI;
        }
        
        private async void Awake()
        {
            var token = await _restAPI.UserLogin(SystemInfo.deviceUniqueIdentifier);
            
            var configData = await _restAPI.GetConfig(token);
            var config = ConfigHelper.GetConfig(configData);
            
            var gameState = await _restAPI.GetState(token);
            
            _sessionData.Setup(token, config, gameState);
            SceneManager.LoadScene("Gameplay_new");
        }
    }
}