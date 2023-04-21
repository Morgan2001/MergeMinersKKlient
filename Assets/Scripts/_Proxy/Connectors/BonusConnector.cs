using _Proxy.Events;
using _Proxy.Services;
using MergeMiner.Core.State.Data;
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
            _addBonusEvent.Trigger(new AddBonusData(gameEvent.Id));
        }
        
        private void OnUseBonus(UseBonusEvent gameEvent)
        {
            double value = 0;
            if (gameEvent.BoostType == BoostType.Money)
            {
                value = _minersBonusHelper.GetMinersBonus(gameEvent.Player);
            }
            _useBonusEvent.Trigger(new UseBonusData(gameEvent.BoostType, value));
        }

        public void UseBonus(string id)
        {
            _playerActionProxy.UseBonus(id);
        }
    }

    public struct AddBonusData
    {
        public string Id { get; }

        public AddBonusData(string id)
        {
            Id = id;
        }
    }
    
    public struct UseBonusData
    {
        public BoostType BoostType { get; }
        public double Value { get; }

        public UseBonusData(BoostType boostType, double value)
        {
            BoostType = boostType;
            Value = value;
        }
    }
}