using _Proxy.Data;
using _Proxy.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Utils;
using Utils;

namespace _Proxy.Connectors
{
    public class RelocateConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly PlayerActionService _playerActionService;
        private readonly RelocateHelper _relocateHelper;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public RelocateConnector(
            LocalPlayer localPlayer,
            PlayerActionService playerActionService,
            RelocateHelper relocateHelper,
            TimerService timerService)
        {
            _localPlayer = localPlayer;
            _playerActionService = playerActionService;
            _relocateHelper = relocateHelper;
            _timerService = timerService;

            _timerService.TickEvent.Subscribe(UpdateProgress);
        }

        public void Relocate()
        {
            _playerActionService.Process(new RelocatePlayerAction(_localPlayer.Id));
        }

        private void UpdateProgress()
        {
            var progress = _relocateHelper.GetRelocationProgress(_localPlayer.Id);
            _progress.Set(progress);
        }
    }
}