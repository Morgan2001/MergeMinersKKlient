using Common.DI;
using GameCore.Commands;
using GameCore.Connectors;
using GameCore.Services;
using MergeMiner.Core.Commands.Base;
using MergeMiner.Core.Launcher;
using MergeMiner.Core.State.Config;

namespace GameCore
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
            _container.RegisterSingleton<MissionsConnector>();
            _container.RegisterSingleton<MinerShopConnector>();
            _container.RegisterSingleton<BonusConnector>();
            _container.RegisterSingleton<ReferralConnector>();
            _container.RegisterSingleton<PopupsConnector>();
            _container.RegisterSingleton<PurchaseConnector>();
        }
    }
}