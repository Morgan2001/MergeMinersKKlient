using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseInitialiser : MonoBehaviour, IStoreListener
{
    public event Action Initialised;

    public List<string> ConsumableIds;
    public string SubscriptionId;

    public IStoreController StoreController { get; private set; }

    public void Construct()
    {
        InitializePurchasing();
    }

    private void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var id in ConsumableIds)
        {
            builder.AddProduct(id, ProductType.Consumable);
        }
        builder.AddProduct(SubscriptionId, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        Initialised?.Invoke();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }
}
