using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.State.Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class RelocateConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly RelocateHelper _relocateHelper;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public RelocateConnector(
            SessionData sessionData,
            PlayerActionProxy playerActionProxy,
            RelocateHelper relocateHelper,
            TimerService timerService)
        {
            _sessionData = sessionData;
            _playerActionProxy = playerActionProxy;
            _relocateHelper = relocateHelper;
            _timerService = timerService;

            _timerService.TickEvent.Subscribe(UpdateProgress);
        }

        public void Relocate()
        {
            _playerActionProxy.Relocate();
        }

        private void UpdateProgress()
        {
            var progress = (float) _relocateHelper.GetRelocationProgress(_sessionData.Token);
            _progress.Set(progress);
        }
    }
}