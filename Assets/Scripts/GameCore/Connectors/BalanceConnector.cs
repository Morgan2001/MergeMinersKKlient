using GameCore.Events;
using GameCore.Preloader;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class BalanceConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly GameConfig _gameConfig;
        private readonly EventSubscriptionService _eventSubscriptionService;
        
        private ReactiveProperty<bool> _balanceEnabled = new();
        public ReactiveProperty<bool> BalanceEnabled => _balanceEnabled;

        public BalanceConnector(
            SessionData sessionData,
            PlayerMinersRepository playerMinersRepository,
            GameConfig gameConfig,
            EventSubscriptionService eventSubscriptionService)
        {
            _sessionData = sessionData;
            _playerMinersRepository = playerMinersRepository;
            _gameConfig = gameConfig;
            _eventSubscriptionService = eventSubscriptionService;

            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInitGame);
            _eventSubscriptionService.Subscribe<MaxLevelIncreasedEvent>(OnMaxLevelIncreased);
        }

        private void OnInitGame(InitGameEvent gameEvent)
        {
            var playerMiners = _playerMinersRepository.Get(_sessionData.Token);
            _balanceEnabled.Set(playerMiners.MaxLevelAchieved >= _gameConfig.BalanceUnlockLevel);
        }

        private void OnMaxLevelIncreased(MaxLevelIncreasedEvent gameEvent)
        {
            _balanceEnabled.Set(gameEvent.Level >= _gameConfig.BalanceUnlockLevel);
        }
    }
}