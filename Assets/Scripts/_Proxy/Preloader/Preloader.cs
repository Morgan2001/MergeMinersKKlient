using MergeMiner.Core.Network.Helpers;
using MergeMiner.Core.State.Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Proxy.Preloader
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
            
            var locationsConfig = await _restAPI.GetLocationsConfig(token);
            var minersConfig = await _restAPI.GetMinersConfig(token);
            
            var config = new GameConfig(
                600, 1,
                ConfigHelper.GetLocationConfig(locationsConfig),
                ConfigHelper.GetMinerConfig(minersConfig),
                ConfigHelper.GetMinerShopConfig(minersConfig),
                ConfigHelper.GetBonusConfig()
            );
            
            var gameState = await _restAPI.GetState(token);
            
            _sessionData.Setup(token, config, gameState);
            SceneManager.LoadScene("Gameplay_new");
        }
    }
}