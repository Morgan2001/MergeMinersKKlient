using GameCore.Events;
using GameCore.Preloader;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class PlayerConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerRepository _playerRepository;
        private readonly EventSubscriptionService _eventSubscriptionService;
        
        private ReactiveEvent<double> _moneyUpdateEvent = new();
        public IReactiveSubscription<double> MoneyUpdateEvent => _moneyUpdateEvent;
        
        private ReactiveEvent<int> _gemsUpdateEvent = new();
        public IReactiveSubscription<int> GemsUpdateEvent => _gemsUpdateEvent;
        
        public PlayerConnector(
            SessionData sessionData,
            PlayerRepository playerRepository,
            EventSubscriptionService eventSubscriptionService)
        {
            _sessionData = sessionData;
            _playerRepository = playerRepository;
            
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInit);
            _eventSubscriptionService.Subscribe<ChangeMoneyEvent>(OnChangeMoney);
            _eventSubscriptionService.Subscribe<ChangeGemsEvent>(OnChangeGems);
        }

        private void OnInit(InitGameEvent gameEvent)
        {
            var player = _playerRepository.Get(_sessionData.Token);
            _moneyUpdateEvent.Trigger(player.Money);
            _gemsUpdateEvent.Trigger(player.Gems);
        }

        private void OnChangeMoney(ChangeMoneyEvent gameEvent)
        {
            _moneyUpdateEvent.Trigger(gameEvent.Money);
        }
        
        private void OnChangeGems(ChangeGemsEvent gameEvent)
        {
            _gemsUpdateEvent.Trigger(gameEvent.Gems);
        }
    }
}