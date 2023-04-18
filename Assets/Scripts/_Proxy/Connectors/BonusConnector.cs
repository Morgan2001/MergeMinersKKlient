using _Proxy.Events;
using _Proxy.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Utils;
using Utils;

namespace _Proxy.Connectors
{
    public class BonusConnector
    {
        private readonly MinersBonusHelper _minersBonusHelper;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly EventSubscriptionService _eventSubscriptionService;

        private ReactiveEvent<AddBonusData> _addBonusEvent = new();
        public IReactiveSubscription<AddBonusData> AddBonusEvent => _addBonusEvent;
        
        private ReactiveEvent<UseBonusData> _useBonusEvent = new();
        public IReactiveSubscription<UseBonusData> UseBonusEvent => _useBonusEvent;

        public BonusConnector(
            MinersBonusHelper minersBonusHelper,
            PlayerActionProxy playerActionProxy,
            EventSubscriptionService eventSubscriptionService)
        {
            _minersBonusHelper = minersBonusHelper;
            _playerActionProxy = playerActionProxy;
            
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
            _playerActionProxy.UseBonus(bonusType);
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