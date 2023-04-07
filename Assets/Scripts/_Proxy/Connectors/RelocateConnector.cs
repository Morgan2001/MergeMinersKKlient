using _Proxy.Data;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.Events.Services;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Utils;
using Utils;

namespace _Proxy.Connectors
{
    public class RelocateConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly EventSubscriptionService _eventSubscriptionService;
        private readonly PlayerActionService _playerActionService;
        private readonly RelocateHelper _relocateHelper;

        private ReactiveProperty<float> _progress = new();
        public ReactiveProperty<float> Progress => _progress;

        public RelocateConnector(
            LocalPlayer localPlayer,
            EventSubscriptionService eventSubscriptionService,
            PlayerActionService playerActionService,
            RelocateHelper relocateHelper)
        {
            _localPlayer = localPlayer;
            _eventSubscriptionService = eventSubscriptionService;
            _playerActionService = playerActionService;
            _relocateHelper = relocateHelper;

            _eventSubscriptionService.Subscribe<AddMinerEvent>(OnAddMiner);
            _eventSubscriptionService.Subscribe<MergeMinersEvent>(OnMergeMiner);
        }

        public void Relocate()
        {
            _playerActionService.Process(new RelocatePlayerAction(_localPlayer.Id));
        }

        private void UpdateProgress()
        {
            _progress.Set(_relocateHelper.GetRelocationProgress(_localPlayer.Id));
        }

        private void OnAddMiner(AddMinerEvent gameEvent)
        {
            UpdateProgress();
        }
        
        private void OnMergeMiner(MergeMinersEvent gameEvent)
        {
            UpdateProgress();
        }
    }
}