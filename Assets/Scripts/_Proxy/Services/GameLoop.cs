using _Proxy.Data;
using MergeMiner.Core.Commands.GameCommands;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Services;
using UnityEngine;

namespace _Proxy.Services
{
    public class GameLoop
    {
        private readonly LocalPlayer _localPlayer;
        private readonly CreatePlayerService _createPlayerService;
        private readonly GameCommandService _gameCommandService;
        private readonly TimerService _timerService;
        private readonly FlyingBonuses _flyingBonuses;

        public GameLoop(
            LocalPlayer localPlayer,
            CreatePlayerService createPlayerService,
            GameCommandService gameCommandService,
            TimerService timerService,
            FlyingBonuses flyingBonuses)
        {
            _localPlayer = localPlayer;
            _createPlayerService = createPlayerService;
            _gameCommandService = gameCommandService;
            _timerService = timerService;
            _flyingBonuses = flyingBonuses;
        }

        public void Init()
        {
            var player = _createPlayerService.CreatePlayer();
            _localPlayer.Set(player);

            _flyingBonuses.Reset();
        }
        
        public void Update()
        {
            _gameCommandService.Process(new CheckSpawnBoxGameCommand(_localPlayer.Id));
            _gameCommandService.Process(new CheckBonusesGameCommand(_localPlayer.Id));
            _gameCommandService.Process(new CalculateGameCommand(_localPlayer.Id));
            _timerService.Tick(Time.deltaTime);
        }
    }
}