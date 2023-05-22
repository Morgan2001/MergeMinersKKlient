using System;
using Common.Utils.Misc;
using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.Network.Data;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class PlayerBoxConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerBoxRepository _playerBoxRepository;
        private readonly PlayerSlotsRepository _playerSlotsRepository;
        private readonly LocationConfig _locationConfig;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly TimeService _timeService;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _boxProgress = new();
        public IReactiveProperty<float> BoxProgress => _boxProgress;

        private Action _callback;
        
        public PlayerBoxConnector(
            SessionData sessionData,
            PlayerBoxRepository playerBoxRepository,
            PlayerSlotsRepository playerSlotsRepository,
            LocationConfig locationConfig,
            PlayerActionProxy playerActionProxy,
            TimeService timeService,
            TimerService timerService)
        {
            _sessionData = sessionData;
            _playerBoxRepository = playerBoxRepository;
            _playerSlotsRepository = playerSlotsRepository;
            _locationConfig = locationConfig;
            _playerActionProxy = playerActionProxy;
            _timeService = timeService;

            _timerService = timerService;
            _timerService.TickEvent.Subscribe(OnTickEvent);
        }

        public void SpeedUp()
        {
            _playerActionProxy.SpeedUpBlueBox();
        }

        private void OnTickEvent()
        {
            if (!_sessionData.Working)
            {
                _boxProgress.Set(1f);
                return;
            }

            var playerBox = _playerBoxRepository.Get(_sessionData.Token);
            var playerSlots = _playerSlotsRepository.Get(_sessionData.Token);
            var location = _locationConfig.GetLocation(playerSlots.Level);

            var timeDelta = playerBox.SpawnTime - _timeService.GetCurrentTime;
            var timeDeltaSeconds = (double) timeDelta / TimeSpan.TicksPerSecond;
            
            var progress = 1f - (float) (timeDeltaSeconds / location.SpawnDelay);
            _boxProgress.Set(progress);

            if (progress < 1f) return;
            if (_callback != null) return;
                
            _callback = OnSpawn;
            _playerActionProxy.SpawnBox(_callback);
        }

        private void OnSpawn()
        {
            _callback = null;
        }
    }
}