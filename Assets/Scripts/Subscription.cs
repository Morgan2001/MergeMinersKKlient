using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Subscription : MonoBehaviour
{
    public event Action InitComplete;

    public string SubscriptionProductId = "com.mogames.get.crypto.nft.game.subscription";
    public SubscriptionData Data;

    private SubscriptionManager subscriptionManager;
    private Product subscription;

    private PurchaseInitialiser purchaseInitialiser;

    public void Construct(PurchaseInitialiser purchaseInitialiser)
    {
        this.purchaseInitialiser = purchaseInitialiser;


        if (purchaseInitialiser.StoreController == null)
        {
            purchaseInitialiser.Initialised += Init;
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        subscription = purchaseInitialiser.StoreController.products.WithID(SubscriptionProductId);
        subscriptionManager = new SubscriptionManager(subscription, null);

        purchaseInitialiser.Initialised -= Init;

        InitComplete?.Invoke();
    }

    public bool IsActive()
    {
        try
        {
            if (subscription == null || !subscription.hasReceipt)
            {
                return false;
            }
            else
            {
                return subscriptionManager.getSubscriptionInfo().isSubscribed() == Result.True;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Ex in IsActive Sub: " + ex.Message);
            return false;
        }
    }

    // FOR DEBUG
    public void GetSubState()
    {
        try
        {
            Debug.Log($"subscription.hasReceipt: {subscription.hasReceipt},  SubscriptionInfo().isSubscribed() {subscriptionManager.getSubscriptionInfo().isSubscribed()}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"ERROR: {ex.Message}");
        }
    }
}
