using _Proxy.Services;
using Common.DI;
using MergeMiner.Core.State.Config;

namespace _Proxy
{
    public class Launcher
    {
        private IServiceProvider _serviceProvider;
        
        private readonly GameLoop _gameLoop;
        public GameLoop GameLoop => _serviceProvider.Resolve<GameLoop>();
        
        public Launcher(
            IServiceFactory serviceFactory,
            GameConfig gameConfig)
        {
            var installer = new LocalAppInstaller(serviceFactory, gameConfig);
            _serviceProvider = installer.Install();
        }
    }
}