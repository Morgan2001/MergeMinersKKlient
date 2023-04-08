using System.Collections.Generic;
using System.Linq;
using _Proxy.Connectors;
using _Proxy.Services;
using Common.DI;
using MergeMiner.Core.State.Config;
using UnityEngine;
using Zenject;

namespace _Proxy
{
    public class StartUp : MonoInstaller
    {
        [SerializeField] private GameRules _gameRules;

        private IServiceProvider _serviceProvider;
        
        private GameLoop _gameLoop;

        private void Awake()
        {
            _gameLoop.Init();
        }

        public override void InstallBindings()
        {
            Setup();
            
            Bind<PlayerConnector>();
            Bind<PlayerBoxConnector>();
            Bind<MinerFieldConnector>();
            Bind<RelocateConnector>();
            Bind<FreeGemConnector>();
            Bind<MinerShopConnector>();
        }
        
        private void Bind<T>()
            where T : class
        {
            var instance = _serviceProvider.Resolve<T>();
            Container.BindInstance(instance);
        }

        private void Setup()
        {
            var container = new ZenjectContainer();
            var launcher = new Launcher(container,
                new GameConfig(),
                GetLocationConfig(),
                GetMinerConfig(),
                GetMinerShopConfig());

            _serviceProvider = launcher.ServiceProvider;
            _gameLoop = launcher.GameLoop;
        }

        private LocationConfig GetLocationConfig()
        {
            var items = _gameRules.Locations.LocationDatas
                .Select(x => new KeyValuePair<int, LocationConfigItem>(x.Level,
                        new LocationConfigItem(
                            x.Name,
                            x.NumOfMiningCells,
                            x.NumOfColumns,
                            x.NumOfRows,
                            x.MaxLevelOfMiningDevice,
                            x.LevelOfMiningDeviceFromBox,
                            8, 1, 
                            (long) x.RelocateCost, 
                            x.MinMiningDeviceLevelToRelocate)
                    )
                );
            var config = new LocationConfig(new Dictionary<int, string>(items.Select(x => new KeyValuePair<int, string>(x.Key, x.Value.Id))));
            foreach (var item in items)
            {
                config.Add(item.Value);
            }
            return config;
        }
        
        private MinerConfig GetMinerConfig()
        {
            var items = _gameRules.MiningDevices.MiningDeviceDatas
                .Select(x => new KeyValuePair<int, MinerConfigItem>(x.Level,
                        new MinerConfigItem(
                            x.Name,
                            x.Level,
                            (long) x.CoinsPerSecond,
                            (long) x.BuyPrice,
                            1.0623f,
                            100,
                            0)
                    )
                );
            var config = new MinerConfig(new Dictionary<int, string>(items.Select(x => new KeyValuePair<int, string>(x.Key, x.Value.Id))), 10);
            foreach (var item in items)
            {
                config.Add(item.Value);
            }
            return config;
        }
        
        private MinerShopConfig GetMinerShopConfig()
        {
            var miners = _gameRules.MiningDevices.MiningDeviceDatas;
            var items = _gameRules.MinerShop.ForMoney
                .Select(x =>
                {
                    var miner = miners.First(m => m.Level == x.MiningDeviceLevel);
                    return new KeyValuePair<int, MinerShopConfigItem>(x.Id,
                        new MinerShopConfigItem(
                            miner.Name,
                            miner.Name,
                            x.MaxAchivedMinerLevelRequired)
                    );
                });
            var config = new MinerShopConfig();
            foreach (var item in items)
            {
                config.Add(item.Value);
            }
            return config;
        }

        private void Update()
        {
            _gameLoop.Update();
        }
    }
}