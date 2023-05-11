using System;
using MergeMiner.Core.Commands.Base;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Services.State;

namespace GameCore.Commands
{
    public class MergeMinersCommand : GameCommand
    {
        public int Slot1 { get; }
        public int Slot2 { get; }
        
        public string Miner { get; }
        public string MinerConfig { get; }
        
        public MergeMinersCommand(string player, int slot1, int slot2, string miner, string minerConfig) : base(player)
        {
            Slot1 = slot1;
            Slot2 = slot2;
            Miner = miner;
            MinerConfig = minerConfig;
        }
    }
    
    public class MergeMinersCommandProcessor : GameCommandProcessor<MergeMinersCommand>
    {
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly PlayerMinersStateService _playerMinersStateService;
        private readonly MinerPoolService _minerPoolService;
        private readonly MinerConfig _minerConfig;
        private readonly StatsService _statsService;
        private readonly EventDispatcherService _eventDispatcherService;

        public MergeMinersCommandProcessor(
            PlayerMinersRepository playerMinersRepository,
            PlayerMinersStateService playerMinersStateService,
            MinerPoolService minerPoolService,
            MinerConfig minerConfig,
            StatsService statsService,
            EventDispatcherService eventDispatcherService)
        {
            _playerMinersRepository = playerMinersRepository;
            _playerMinersStateService = playerMinersStateService;
            _minerPoolService = minerPoolService;
            _minerConfig = minerConfig;
            _statsService = statsService;
            _eventDispatcherService = eventDispatcherService;
        }
        
        protected override void Process(MergeMinersCommand gameCommand)
        {
            var playerMiners = _playerMinersRepository.Get(gameCommand.Player);
            
            var minerId1 = playerMiners.Miners[gameCommand.Slot1];
            if (minerId1 == null) throw new Exception("Can't merge empty miner");
            
            var minerId2 = playerMiners.Miners[gameCommand.Slot2];
            if (minerId2 == null) throw new Exception("Can't merge empty miner");
            
            var newMiner = _minerPoolService.TakeMiner(gameCommand.MinerConfig, gameCommand.Miner);
            var minerConfig = _minerConfig.Get(gameCommand.MinerConfig);
            var newLevel = _playerMinersStateService.SetMaxLevelAchieved(gameCommand.Player, minerConfig.Level);
            if (newLevel)
            {
                _eventDispatcherService.Dispatch(new MaxLevelIncreasedEvent(gameCommand.Player, minerConfig.Level));
            }
            
            _playerMinersStateService.SetMiner(gameCommand.Player, newMiner.Id, gameCommand.Slot2);
            _playerMinersStateService.RemoveMiner(gameCommand.Player, gameCommand.Slot1);
            _eventDispatcherService.Dispatch(new MergeMinersEvent(gameCommand.Player, newMiner.Id, gameCommand.Slot2, newLevel, minerId1, minerId2));
            
            _statsService.IncrementMission(gameCommand.Player, minerConfig.Level, MissionType.Create);
            
            _minerPoolService.ReturnMiner(minerId1);
            _minerPoolService.ReturnMiner(minerId2);
        }
    }
}