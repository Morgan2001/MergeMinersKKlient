using GameCore.Services;
using MergeMiner.Core.State.Config;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class ShopConnector
    {
        private readonly IAPConfig _iapConfig;
        private readonly PlayerActionProxy _playerActionProxy;

        private ReactiveEvent<InitIAPsData> _initIAPsEvent = new();
        public IReactiveSubscription<InitIAPsData> InitIAPsEvent => _initIAPsEvent;

        public ShopConnector(
            IAPConfig iapConfig,
            PlayerActionProxy playerActionProxy)
        {
            _iapConfig = iapConfig;
            _playerActionProxy = playerActionProxy;
        }

        public void Restore()
        {
            _initIAPsEvent.Trigger(new InitIAPsData(_iapConfig));
        }

        public void InitPurchase(string id)
        {
            _playerActionProxy.PurchaseTest(id);
        }

        public void InitSubscription()
        {
            _playerActionProxy.PurchaseTestSubscription();
        }
    }

    public class InitIAPsData
    {
        public IAPConfig IAPs { get; }

        public InitIAPsData(IAPConfig iaPs)
        {
            IAPs = iaPs;
        }
    }
}