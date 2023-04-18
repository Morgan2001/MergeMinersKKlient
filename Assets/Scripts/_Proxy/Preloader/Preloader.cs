using System.Threading.Tasks;
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
            if (!PlayerPrefs.HasKey("token"))
            {
                await Register();
            }

            while (true)
            {
                var token = PlayerPrefs.GetString("token");
                var gameState = await _restAPI.GetState(token);
                if (gameState == null)
                {
                    await Register();
                }
                else
                {
                    _sessionData.Setup(token, gameState);
                    SceneManager.LoadScene("Gameplay_new");
                    return;
                }
            }
        }

        private async Task Register()
        {
            var token = await _restAPI.UserRegister();
            PlayerPrefs.SetString("token", token);
            PlayerPrefs.Save();
        }
    }
}