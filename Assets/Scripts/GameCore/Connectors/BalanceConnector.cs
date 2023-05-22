using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class BalanceConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly GameConfig _gameConfig;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public BalanceConnector(
            SessionData sessionData,
            PlayerMinersRepository playerMinersRepository,
            GameConfig gameConfig,
            TimerService timerService)
        {
            _sessionData = sessionData;
            _playerMinersRepository = playerMinersRepository;
            _gameConfig = gameConfig;
            _timerService = timerService;

            _timerService.TickEvent.Subscribe(UpdateProgress);
        }

        private void UpdateProgress()
        {
            var playerMiner = _playerMinersRepository.Get(_sessionData.Token);
            var progress = (float) playerMiner.MaxLevelAchieved / _gameConfig.BalanceUnlockLevel;
            _progress.Set(progress);
        }
    }
}