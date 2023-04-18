using _Proxy.Preloader;
using MergeMiner.Core.Commands.GameCommands;
using MergeMiner.Core.Commands.Services;
using UnityEngine;

namespace _Proxy.Services
{
    public class GameLoop
    {
        private readonly SessionData _sessionData;
        private readonly CreatePlayerService _createPlayerService;
        private readonly GameCommandService _gameCommandService;
        private readonly TimerService _timerService;
        private readonly FlyingBonuses _flyingBonuses;
        private readonly GameStateApplier _gameStateApplier;

        public GameLoop(
            SessionData sessionData,
            CreatePlayerService createPlayerService,
            GameCommandService gameCommandService,
            TimerService timerService,
            FlyingBonuses flyingBonuses,
            GameStateApplier gameStateApplier)
        {
            _sessionData = sessionData;
            _createPlayerService = createPlayerService;
            _gameCommandService = gameCommandService;
            _timerService = timerService;
            _flyingBonuses = flyingBonuses;
            _gameStateApplier = gameStateApplier;
        }

        public void Init()
        {
            _createPlayerService.CreatePlayer(_sessionData.Token);
            _createPlayerService.SetupPlayer(_sessionData.Token, _sessionData.GameState.Slots.Level);

            _gameStateApplier.Apply(_sessionData.GameState, _sessionData.Token);
            
            _flyingBonuses.Reset();
        }
        
        public void Update()
        {
            _gameCommandService.Process(new CheckBonusesGameCommand(_sessionData.Token));
            _gameCommandService.Process(new CalculateGameCommand(_sessionData.Token));
            _timerService.Tick(Time.deltaTime);
        }
    }
}