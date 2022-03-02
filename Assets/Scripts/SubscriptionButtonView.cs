using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubscriptionButtonView : MonoBehaviour
{
    public Text SubscribeText;
    public Text PriceText;
    public Text AlreadySubscribedText;

    private Subscription subscription;

    public void Construct(Subscription subscription)
    {
        this.subscription = subscription;
    }

    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (subscription.IsActive())
        {
            SubscribeText.gameObject.SetActive(false);
            PriceText.gameObject.SetActive(false);
            AlreadySubscribedText.gameObject.SetActive(true);
        }
        else
        {
            SubscribeText.gameObject.SetActive(true);
            PriceText.gameObject.SetActive(true);
            AlreadySubscribedText.gameObject.SetActive(false);
        }
    }
}
