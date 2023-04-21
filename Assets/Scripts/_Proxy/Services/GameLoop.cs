using _Proxy.Events;
using _Proxy.Preloader;
using MergeMiner.Core.Commands.GameCommands;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.State.Services;
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
            
            _eventDispatcherService.Dispatch(new InitGameEvent(_sessionData.Token, true));
        }
        
        public void Update()
        {
            if (!_sessionData.Started) return;
            
            _gameCommandService.Process(new CheckBonusesGameCommand(_sessionData.Token));
            _gameCommandService.Process(new CalculateGameCommand(_sessionData.Token));
            _timerService.Tick(Time.deltaTime);
        }
    }
}