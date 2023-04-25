using GameCore.Connectors;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;
using Zenject;

namespace UI.ShopScreen
{
    public class ShopPanelSetup : MonoBehaviour
    {
        [SerializeField] private ShopPurchaseView _shopPurchasePrefab;
        [SerializeField] private Transform _shopPurchaseContainer;

        [SerializeField] private Button _rouletteButton;
        [SerializeField] private Button _subscriptionButton;
        
        [SerializeField] private GameObject _subscriptionActive;
        [SerializeField] private GameObject _subscriptionNotActive;

        private PurchaseConnector _purchaseConnector;
        private PopupsConnector _popupsConnector;

        [Inject]
        private void Setup(
            PurchaseConnector purchaseConnector,
            PopupsConnector popupsConnector)
        {
            _purchaseConnector = purchaseConnector;
            _purchaseConnector.AddProductEvent.Subscribe(OnInitIAPs);
            _purchaseConnector.SubscriptionEvent.Subscribe(OnSubscription);

            _popupsConnector = popupsConnector;

            _rouletteButton.Subscribe(_popupsConnector.SpinWheel);
            _subscriptionButton.Subscribe(_purchaseConnector.InitSubscription);
            
            UpdateSubscription(false);
        }

        private void OnInitIAPs(ProductsData data)
        {
            foreach (var item in data.Products)
            {
                var id = item.Id;
                var viewModel = new ShopPurchaseViewModel(item.Gems, item.Price);
                var view = Instantiate(_shopPurchasePrefab, _shopPurchaseContainer);
                view.Bind(viewModel);
                
                view.ClickEvent.Subscribe(() => _purchaseConnector.InitPurchase(id)).AddTo(view);
            }
        }

        private void OnSubscription()
        {
            UpdateSubscription(true);
        }

        private void UpdateSubscription(bool value)
        {
            _subscriptionActive.SetActive(value);
            _subscriptionNotActive.SetActive(!value);
            _subscriptionButton.interactable = !value;
        }
    }
}