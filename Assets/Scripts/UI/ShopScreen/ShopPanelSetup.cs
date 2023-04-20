using _Proxy.Connectors;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Zenject;

namespace UI.ShopScreen
{
    public class ShopPanelSetup : MonoBehaviour
    {
        [SerializeField] private ShopPurchaseView _shopPurchasePrefab;
        [SerializeField] private Transform _shopPurchaseContainer;

        [SerializeField] private Button _rouletteButton;
        [SerializeField] private Button _subscriptionButton;

        private ShopConnector _shopConnector;
        private PopupsConnector _popupsConnector;

        [Inject]
        private void Setup(
            ShopConnector shopConnector,
            PopupsConnector popupsConnector)
        {
            _shopConnector = shopConnector;
            _shopConnector.InitIAPsEvent.Subscribe(OnInitIAPs);

            _popupsConnector = popupsConnector;

            _rouletteButton.Subscribe(_popupsConnector.RollRoulette);
            _subscriptionButton.Subscribe(_shopConnector.InitSubscription);
        }

        private void Awake()
        {
            _shopConnector.Restore();
        }

        private void OnInitIAPs(InitIAPsData data)
        {
            foreach (var item in data.IAPs.GetAll())
            {
                var id = item.Id;
                var viewModel = new ShopPurchaseViewModel(item.Gems, item.Price);
                var view = Instantiate(_shopPurchasePrefab, _shopPurchaseContainer);
                view.Bind(viewModel);
                
                view.ClickEvent.Subscribe(() => _shopConnector.InitPurchase(id)).AddTo(view);
            }
        }
    }
}