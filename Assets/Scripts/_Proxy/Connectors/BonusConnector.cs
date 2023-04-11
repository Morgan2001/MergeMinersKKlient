using _Proxy.Data;
using _Proxy.Events;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Utils;
using Utils;

namespace _Proxy.Connectors
{
    public class BonusConnector
    {
        private readonly LocalPlayer _localPlayer;
        private readonly MinersBonusHelper _minersBonusHelper;
        private readonly PlayerActionService _playerActionService;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<AddBonusData> _addBonusEvent = new();
        public IReactiveSubscription<AddBonusData> AddBonusEvent => _addBonusEvent;
        
        private ReactiveEvent<UseBonusData> _useBonusEvent = new();
        public IReactiveSubscription<UseBonusData> UseBonusEvent => _useBonusEvent;

        public BonusConnector(
            LocalPlayer localPlayer,
            MinersBonusHelper minersBonusHelper,
            PlayerActionService playerActionService,
            EventSubscriptionService eventSubscriptionService)
        {
            _localPlayer = localPlayer;
            _minersBonusHelper = minersBonusHelper;
            _playerActionService = playerActionService;
            
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<AddBonusEvent>(OnAddBonus);
            _eventSubscriptionService.Subscribe<UseBonusEvent>(OnUseBonus);
        }

        private void OnAddBonus(AddBonusEvent gameEvent)
        {
            _addBonusEvent.Trigger(new AddBonusData(gameEvent.BonusType));
        }
        
        private void OnUseBonus(UseBonusEvent gameEvent)
        {
            double value = 0;
            if (gameEvent.BonusType == BonusType.Money)
            {
                value = _minersBonusHelper.GetMinersBonus(gameEvent.Player);
            }
            _useBonusEvent.Trigger(new UseBonusData(gameEvent.BonusType, value));
        }

        public void UseBonus(BonusType bonusType)
        {
            _playerActionService.Process(new UseBonusPlayerAction(_localPlayer.Id, bonusType));
        }
    }

    public struct AddBonusData
    {
        public BonusType BonusType { get; }

        public AddBonusData(BonusType bonusType)
        {
            BonusType = bonusType;
        }
    }
    
    public struct UseBonusData
    {
        public BonusType BonusType { get; }
        public double Value { get; }

        public UseBonusData(BonusType bonusType, double value)
        {
            BonusType = bonusType;
            Value = value;
        }
    }
}