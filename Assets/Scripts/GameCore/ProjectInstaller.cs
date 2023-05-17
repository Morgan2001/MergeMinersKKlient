using GameCore.Connectors;
using GameCore.Preloader;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameCore
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SessionData>().AsSingle();
            Container.Bind<RestClient>().AsSingle();
            Container.Bind<RestAPI>().AsSingle();
            Container.Bind<AlertConnector>().AsSingle();
        }

        public override void Start()
        {
            SceneManager.LoadScene("Preloader");
        }
    }
}