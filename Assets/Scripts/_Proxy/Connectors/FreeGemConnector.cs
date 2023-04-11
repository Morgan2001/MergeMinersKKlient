using _Proxy.Data;
using _Proxy.Services;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using Utils;

namespace _Proxy.Connectors
{
    public class FreeGemConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly PlayerActionService _playerActionService;
        private readonly FreeGemService _freeGemService;
        private readonly TimerService _timerService;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public FreeGemConnector(
            LocalPlayer localPlayer,
            PlayerActionService playerActionService,
            FreeGemService freeGemService,
            TimerService timerService)
        {
            _localPlayer = localPlayer;
            _playerActionService = playerActionService;
            _freeGemService = freeGemService;
            _timerService = timerService;

            _timerService.TickEvent.Subscribe(OnTick);
        }

        public void GetFreeGem()
        {
            _playerActionService.Process(new GetFreeGemPlayerAction(_localPlayer.Id));
        }

        private void OnTick()
        {
            _progress.Set(_freeGemService.GetGemProgress(_localPlayer.Id));
        }
    }
}