using _Proxy.Services;
using Common.DI;
using MergeMiner.Core.State.Config;

namespace _Proxy
{
    public class Launcher
    {
        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;
        
        private readonly GameLoop _gameLoop;
        public GameLoop GameLoop => _gameLoop;
        
        public Launcher(
            IServiceFactory serviceFactory, 
            GameConfig gameConfig)
        {
            var installer = new LocalAppInstaller(serviceFactory, gameConfig);
            _serviceProvider = installer.Install();
            _gameLoop = _serviceProvider.Resolve<GameLoop>();
        }
    }
}