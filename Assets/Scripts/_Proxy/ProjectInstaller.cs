using _Proxy.Preloader;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Proxy
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SessionData>().AsSingle();
            Container.Bind<RestClient>().AsSingle();
            Container.Bind<RestAPI>().AsSingle();
        }

        public override void Start()
        {
            SceneManager.LoadScene("Preloader");
        }
    }
}