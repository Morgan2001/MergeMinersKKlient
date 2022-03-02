using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ads : MonoBehaviour
{
    public string AppKey = "12e93c2ad";

    private Action curRewardAction;

    private Subscription subscription;

    public void Construct(Subscription subscription)
    {
        this.subscription = subscription;
        IronSource.Agent.init(AppKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
        
        InitInterstitional();
        InitRewarded();
    }

    private void InitRewarded()
    {
        IronSourceEvents.onRewardedVideoAdRewardedEvent += (placement) => curRewardAction?.Invoke();
    }

    private void InitInterstitional()
    {
        if (!SubscriptionRemovesAds())
        {
            IronSourceEvents.onInterstitialAdClosedEvent += IronSource.Agent.loadInterstitial;
            IronSource.Agent.loadInterstitial();
        }
    }

    public void ShowInterstitial()
    {
        if (!SubscriptionRemovesAds() && IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
    }

    public void ShowRewarded(Action rewardAction)
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            curRewardAction = rewardAction;

            IronSource.Agent.showRewardedVideo();
        }
    }

    public bool SubscriptionRemovesAds()
    {
        return subscription.IsActive() && subscription.Data.AdsRemove;
    }
}
