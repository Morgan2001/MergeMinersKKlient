using _Proxy.Data;
using MergeMiner.Core.Commands.GameCommands;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Services;

namespace _Proxy.Services
{
    public class GameLoop
    {
        private readonly LocalPlayer _localPlayer;
        private readonly CreatePlayerService _createPlayerService;
        private readonly GameCommandService _gameCommandService;
        private readonly TimerService _timerService;

        public GameLoop(
            LocalPlayer localPlayer,
            CreatePlayerService createPlayerService,
            GameCommandService gameCommandService,
            TimerService timerService)
        {
            _localPlayer = localPlayer;
            _createPlayerService = createPlayerService;
            _gameCommandService = gameCommandService;
            _timerService = timerService;
        }

        public void Init()
        {
            var player = _createPlayerService.CreatePlayer();
            _localPlayer.Set(player);
        }
        
        public void Update()
        {
            _gameCommandService.Process(new SpawnBoxGameCommand(_localPlayer.Id));
            _gameCommandService.Process(new CalculateGameCommand(_localPlayer.Id));
            _timerService.Tick();
        }
    }
}