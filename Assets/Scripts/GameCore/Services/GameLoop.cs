using GameCore.Events;
using GameCore.Preloader;
using MergeMiner.Core.Commands.GameCommands;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Services;
using UnityEngine;

namespace GameCore.Services
{
    public class GameLoop
    {
        private readonly SessionData _sessionData;
        private readonly CreatePlayerService _createPlayerService;
        private readonly GameCommandService _gameCommandService;
        private readonly TimerService _timerService;
        private readonly FlyingBonuses _flyingBonuses;
        private readonly GameStateApplier _gameStateApplier;
        private readonly EventDispatcherService _eventDispatcherService;

        public GameLoop(
            SessionData sessionData,
            CreatePlayerService createPlayerService,
            GameCommandService gameCommandService,
            TimerService timerService,
            FlyingBonuses flyingBonuses,
            GameStateApplier gameStateApplier,
            EventDispatcherService eventDispatcherService)
        {
            _sessionData = sessionData;
            _createPlayerService = createPlayerService;
            _gameCommandService = gameCommandService;
            _timerService = timerService;
            _flyingBonuses = flyingBonuses;
            _gameStateApplier = gameStateApplier;
            _eventDispatcherService = eventDispatcherService;
        }

        public void Init()
        {
            _createPlayerService.CreatePlayer(_sessionData.Token);
            _createPlayerService.SetupPlayer(_sessionData.Token, _sessionData.GameState.Slots.Level);

            _gameStateApplier.Apply(_sessionData.GameState, _sessionData.Token);
            
            _flyingBonuses.Reset();

            var newPlayer = _sessionData.GameState.Player.Money == 0;
            _eventDispatcherService.Dispatch(new InitGameEvent(_sessionData.Token, !newPlayer));
            if (newPlayer)
            {
                _sessionData.SetWorking(true);
            }
        }
        
        public void Update()
        {
            _timerService.Tick(Time.deltaTime);
            
            if (!_sessionData.Working) return;
            
            _gameCommandService.Process(new CheckBonusesGameCommand(_sessionData.Token));
            _gameCommandService.Process(new CalculateGameCommand(_sessionData.Token));
        }
    }
}