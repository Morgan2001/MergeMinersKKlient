using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.Commands.Services;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class FreeGemConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly FreeGemService _freeGemService;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public FreeGemConnector(
            SessionData sessionData,
            PlayerActionProxy playerActionProxy,
            FreeGemService freeGemService,
            TimerService timerService)
        {
            _sessionData = sessionData;
            _playerActionProxy = playerActionProxy;
            _freeGemService = freeGemService;
            _timerService = timerService;

            _timerService.TickEvent.Subscribe(OnTick);
        }

        public void GetFreeGem()
        {
            _playerActionProxy.GetFreeGem();
        }

        private void OnTick()
        {
            _progress.Set(_freeGemService.GetGemProgress(_sessionData.Token));
        }
    }
}