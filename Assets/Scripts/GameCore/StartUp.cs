using GameCore.Preloader;
using GameCore.Services;
using Zenject;

namespace GameCore
{
    public class StartUp : MonoInstaller
    {
        private GameLoop _gameLoop;

        private void Awake()
        {
            _gameLoop.Init();
        }

        public override void InstallBindings()
        {
            var sessionData = Container.Resolve<SessionData>();
            
            var container = new ZenjectContainer(Container);
            var launcher = new Launcher(container, sessionData.GameConfig);
            _gameLoop = launcher.GameLoop;
        }

        private void Update()
        {
            _gameLoop.Update();
        }
    }
}