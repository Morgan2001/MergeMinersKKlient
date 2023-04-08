using _Proxy.Connectors;
using _Proxy.Data;
using _Proxy.Services;
using Common.DI;
using MergeMiner.Core.Launcher;
using MergeMiner.Core.State.Config;

namespace _Proxy
{
    public class LocalAppInstaller : GameInstaller
    {
        public LocalAppInstaller(
            IServiceFactory factory, 
            GameConfig gameConfig,
            LocationConfig locationConfig, 
            MinerConfig minerConfig, 
            MinerShopConfig minerShopConfig) : base(factory, gameConfig, locationConfig, minerConfig, minerShopConfig)
        {
        }
        
        protected override void InstallBindings()
        {
            base.InstallBindings();
            
            _container.RegisterSingleton<LocalPlayer>();
            
            _container.RegisterSingleton<GameLoop>();
            
            _container.RegisterSingleton<TimerService>();
            
            _container.RegisterSingleton<PlayerConnector>();
            _container.RegisterSingleton<PlayerBoxConnector>();
            _container.RegisterSingleton<MinerFieldConnector>();
            _container.RegisterSingleton<RelocateConnector>();
            _container.RegisterSingleton<FreeGemConnector>();
            _container.RegisterSingleton<MinerShopConnector>();
        }
    }
}