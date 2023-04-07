using System;
using _Proxy.Data;
using _Proxy.Services;
using Common.Utils.Misc;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Repository;
using Utils;

namespace _Proxy.Connectors
{
    public class PlayerBoxConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly PlayerBoxRepository _playerBoxRepository;
        private readonly PlayerSlotsRepository _playerSlotsRepository;
        private readonly LocationConfig _locationConfig;
        private readonly PlayerActionService _playerActionService;
        private readonly TimeService _timeService;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _boxProgress = new();
        public IReactiveProperty<float> BoxProgress => _boxProgress;
        
        public PlayerBoxConnector(
            LocalPlayer localPlayer,
            PlayerBoxRepository playerBoxRepository,
            PlayerSlotsRepository playerSlotsRepository,
            LocationConfig locationConfig,
            PlayerActionService playerActionService,
            TimeService timeService,
            TimerService timerService)
        {
            _localPlayer = localPlayer;
            _playerBoxRepository = playerBoxRepository;
            _playerSlotsRepository = playerSlotsRepository;
            _locationConfig = locationConfig;
            _playerActionService = playerActionService;
            _timeService = timeService;

            _timerService = timerService;
            _timerService.TickEvent.Subscribe(OnTickEvent);
        }

        public void SpeedUp()
        {
            _playerActionService.Process(new SpeedUpBlueBoxPlayerAction(_localPlayer.Id));
        }

        private void OnTickEvent()
        {
            var playerBox = _playerBoxRepository.Get(_localPlayer.Id);
            var playerSlots = _playerSlotsRepository.Get(_localPlayer.Id);
            var location = _locationConfig.GetLocation(playerSlots.Level);

            var timeDelta = playerBox.SpawnTime - _timeService.GetCurrentTime;
            var timeDeltaSeconds = (double) timeDelta / TimeSpan.TicksPerSecond;
            
            var progress = 1f - (float) (timeDeltaSeconds / location.SpawnDelay);
            _boxProgress.Set(progress);
        }
    }
}