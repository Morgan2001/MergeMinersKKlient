using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace IAP
{
    public class StoreListener : IStoreListener
    {
        private IStoreController _storeController;
        public IStoreController StoreController => _storeController;

        public event Action<PurchaseResult> OnPurchase; 
        public event Action<PurchaseResult> OnSubscription; 

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized");
            _storeController = controller;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed: " + error.ToString("G"));
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log("OnInitializeFailed: " + error.ToString("G") + " " + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
#if UNITY_EDITOR
            return PurchaseProcessingResult.Complete;
#endif
#if UNITY_ANDROID
            var receipt = purchaseEvent.purchasedProduct.receipt;
            var purchaseReceipt = JsonUtility.FromJson<GooglePurchaseReceipt>(receipt);
            var purchasePayload = JsonUtility.FromJson<GooglePurchasePayload>(purchaseReceipt.Payload);
            var purchaseData = JsonUtility.FromJson<GooglePurchaseData>(purchasePayload.json);
            
            Debug.Log("ProcessPurchase");
            switch (purchaseEvent.purchasedProduct.definition.type)
            {
                case ProductType.Consumable:
                case ProductType.NonConsumable:
                {
                    OnPurchase?.Invoke(new PurchaseResult(purchaseData.productId, purchaseData.purchaseToken));
                    break;
                }
                
                case ProductType.Subscription:
                {
                    OnSubscription?.Invoke(new PurchaseResult(null, purchaseData.purchaseToken));
                    break;
                }
            }
            return PurchaseProcessingResult.Pending;
#endif
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log("OnPurchaseFailed: " + failureReason.ToString("G"));
        }
        
        private struct GooglePurchaseReceipt
        {
            public string Payload;
        }
        
        private struct GooglePurchasePayload
        {
            public string json;
            public string signature;
        }

        private struct GooglePurchaseData
        {
            public bool autoRenewing;
            public string orderId;
            public string packageName;
            public string productId;
            public long purchaseTime;
            public int purchaseState;
            public string developerPayload;
            public string purchaseToken;
        }
    }

    public struct PurchaseResult
    {
        public string Product { get; }
        public string Token { get; }

        public PurchaseResult(string product, string token)
        {
            Product = product;
            Token = token;
        }
    }
}