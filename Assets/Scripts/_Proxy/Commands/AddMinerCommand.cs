using MergeMiner.Core.Commands.Base;
using MergeMiner.Core.State.Enums;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Services.State;

namespace _Proxy.Commands
{
    public class AddMinerCommand : GameCommand
    {
        public string MinerConfig { get; }
        public int Slot { get; }
        public MinerSource Source { get; }

        public AddMinerCommand(string player, string minerConfig, int slot, MinerSource source) : base(player)
        {
            MinerConfig = minerConfig;
            Slot = slot;
            Source = source;
        }
    }
    
    public class AddMinerCommandProcessor : GameCommandProcessor<AddMinerCommand>
    {
        private readonly SpawnBoxService _spawnBoxService;
        private readonly PlayerMinersStateService _playerMinersStateService;
        private readonly EventDispatcherService _eventDispatcherService;

        public AddMinerCommandProcessor(
            SpawnBoxService spawnBoxService,
            PlayerMinersStateService playerMinersStateService,
            EventDispatcherService eventDispatcherService)
        {
            _spawnBoxService = spawnBoxService;
            _playerMinersStateService = playerMinersStateService;
            _eventDispatcherService = eventDispatcherService;
        }

        protected override void Process(AddMinerCommand gameCommand)
        {
            var miner = _spawnBoxService.SpawnBox(gameCommand.MinerConfig);
            _playerMinersStateService.SetMiner(gameCommand.Player, miner.Id, gameCommand.Slot);
            _eventDispatcherService.Dispatch(new AddMinerEvent(gameCommand.Player, miner.Id, gameCommand.Source, gameCommand.Slot));
        }
    }
}