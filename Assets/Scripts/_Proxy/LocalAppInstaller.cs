using _Proxy.Commands;
using _Proxy.Connectors;
using _Proxy.Services;
using Common.DI;
using MergeMiner.Core.Commands.Base;
using MergeMiner.Core.Launcher;
using MergeMiner.Core.State.Config;

namespace _Proxy
{
    public class LocalAppInstaller : GameInstaller
    {
        public LocalAppInstaller(
            IServiceFactory factory, 
            GameConfig gameConfig) : base(factory, gameConfig)
        {
        }
        
        protected override void InstallBindings()
        {
            base.InstallBindings();
            
            _container.Register<GameStateApplier>();
            _container.RegisterSingleton<GameLoop>();
            _container.RegisterSingleton<FlyingBonuses>();
            
            _container.RegisterSingleton<TimerService>();
            
            _container.Register<IGameCommandProcessor, AddMinerCommandProcessor>(true);
            _container.Register<IGameCommandProcessor, MergeMinersCommandProcessor>(true);
            
            _container.RegisterSingleton<PlayerActionProxy>();
            _container.RegisterSingleton<PlayerConnector>();
            _container.RegisterSingleton<PlayerBoxConnector>();
            _container.RegisterSingleton<MinerFieldConnector>();
            _container.RegisterSingleton<RelocateConnector>();
            _container.RegisterSingleton<FreeGemConnector>();
            _container.RegisterSingleton<WheelConnector>();
            _container.RegisterSingleton<OfflineIncomeConnector>();
            _container.RegisterSingleton<UpgradesConnector>();
            _container.RegisterSingleton<MinerShopConnector>();
            _container.RegisterSingleton<ShopConnector>();
            _container.RegisterSingleton<BonusConnector>();
            _container.RegisterSingleton<PopupsConnector>();
        }
    }
}