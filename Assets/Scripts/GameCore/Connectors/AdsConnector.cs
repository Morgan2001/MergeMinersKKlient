using System;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class AdsConnector
    {
        private ReactiveEvent _showInterstitialEvent = new();
        public IReactiveSubscription ShowInterstitialEvent => _showInterstitialEvent;
        
        private ReactiveEvent<Action<bool>> _showRewardedEvent = new();
        public IReactiveSubscription<Action<bool>> ShowRewardedEvent => _showRewardedEvent;
        
        public void ShowInterstitial()
        {
            _showInterstitialEvent.Trigger();
        }
        
        public void ShowRewarded(Action<bool> callback)
        {
            _showRewardedEvent.Trigger(callback);
        }
    }
}