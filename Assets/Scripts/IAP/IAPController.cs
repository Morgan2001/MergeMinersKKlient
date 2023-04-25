using GameCore.Connectors;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace IAP
{
    public class IAPController : MonoBehaviour
    {
        private StoreListener _storeListener;
        private PurchaseConnector _purchaseConnector;

        [Inject]
        private void Setup(
            StoreListener storeListener,
            PurchaseConnector purchaseConnector)
        {
            _purchaseConnector = purchaseConnector;

            _purchaseConnector.AddProductEvent.Subscribe(AddProducts);
            _purchaseConnector.InitPurchaseEvent.Subscribe(ProcessPurchase);
            
            _storeListener = storeListener;
            _storeListener.OnPurchase += _purchaseConnector.Purchase;
            _storeListener.OnSubscription += _purchaseConnector.Subscription;
        }

        private async void AddProducts(ProductsData data)
        {
            var options = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var product in data.Products)
            {
                builder.AddProduct(product.Id, product.Consumable ? ProductType.Consumable : ProductType.NonConsumable);
            }
            foreach (var product in data.Subscriptions)
            {
                builder.AddProduct(product.Id, ProductType.Subscription);
            }
            UnityPurchasing.Initialize(_storeListener, builder);
        }

        private void ProcessPurchase(string product)
        {
            _storeListener.StoreController.InitiatePurchase(product);
        }
    }
}