using System;
using CAS;
using GameCore.Connectors;
using UnityEngine;
using Zenject;

namespace Ads
{
    public class AdsController : MonoBehaviour
    {
        private IMediationManager _manager;

        private PurchaseConnector _purchaseConnector;
        private AdsConnector _adsConnector;

        private bool _interstitialAwaits;
        private Action<bool> _rewardedAwaits;
        
        [Inject]
        private void Setup(
            PurchaseConnector purchaseConnector,
            AdsConnector adsConnector)
        {
            _purchaseConnector = purchaseConnector;
            _purchaseConnector.SubscriptionEvent.Subscribe(OnSubscription);

            _adsConnector = adsConnector;
            _adsConnector.ShowInterstitialEvent.Subscribe(ShowInterstitial);
            _adsConnector.ShowRewardedEvent.Subscribe(ShowRewarded);
        }

        private void Awake()
        {
            _manager = MobileAds.BuildManager().Build();
            
            _manager.OnInterstitialAdLoaded += () =>
            {
                if (!_interstitialAwaits) return;
                
                _interstitialAwaits = false;
                _manager.ShowAd(AdType.Interstitial);
            };
            
            _manager.OnRewardedAdLoaded += () =>
            {
                if (_rewardedAwaits == null) return;
                
                _manager.ShowAd(AdType.Rewarded);
            };

            _manager.OnRewardedAdCompleted += () =>
            {
                _rewardedAwaits?.Invoke(true);
                _rewardedAwaits = null;
            };
            
            _manager.OnRewardedAdFailedToShow += _ =>
            {
                _rewardedAwaits?.Invoke(false);
                _rewardedAwaits = null;
            };
            
            _manager.OnRewardedAdClosed += () =>
            {
                _rewardedAwaits = null;
            };
        }

        private void OnSubscription()
        {
            _manager.SetEnableAd(AdType.Interstitial, false);
        }

        private void ShowInterstitial()
        {
            if (_manager.IsReadyAd(AdType.Interstitial))
            {
                _manager.ShowAd(AdType.Interstitial);
            }
            else
            {
                _interstitialAwaits = true;
            }
        }
        
        private void ShowRewarded(Action<bool> callback)
        {
            _rewardedAwaits = callback;
            if (_manager.IsReadyAd(AdType.Rewarded))
            {
                _manager.ShowAd(AdType.Rewarded);
            }
        }
    }
}