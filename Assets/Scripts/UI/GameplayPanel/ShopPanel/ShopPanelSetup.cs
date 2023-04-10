using System.Collections.Generic;
using _Proxy.Connectors;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.GameplayPanel.ShopPanel
{
    public class ShopPanelSetup : MonoBehaviour
    {
        [SerializeField] private ShopPanelView _shopPanelView;

        private MinerShopConnector _minerShopConnector;
        private IMinerResourceHelper _minerResourceHelper;
        private GameplayViewStorage _gameplayViewStorage;
        
        private ShopPanelViewModel _shopPanelViewModel;

        private Dictionary<string, MinerShopViewModel> _minerShopItems = new();

        [Inject]
        private void Setup(
            MinerShopConnector minerShopConnector,
            IMinerResourceHelper minerResourceHelper,
            GameplayViewStorage gameplayViewStorage)
        {
            _minerShopConnector = minerShopConnector;
            _minerResourceHelper = minerResourceHelper;
            _gameplayViewStorage = gameplayViewStorage;
            
            _shopPanelViewModel = new ShopPanelViewModel();
            _shopPanelView.Bind(_shopPanelViewModel);

            _minerShopConnector.AddMinerShopEvent.Subscribe(AddMinerShop).AddTo(_shopPanelView);
            _minerShopConnector.UpdateMinerShopEvent.Subscribe(UpdateMinerShop).AddTo(_shopPanelView);
        }

        private void AddMinerShop(AddMinerShopData data)
        {
            var icon = _minerResourceHelper.GetNormalIconByName(data.Name);
            var viewModel = new MinerShopViewModel(data.Id, data.Level, icon);
            _shopPanelViewModel.AddMiner(viewModel);
            _minerShopItems.Add(data.Id, viewModel);
            
            viewModel.ClickEvent.Subscribe(() => _minerShopConnector.BuyMiner(viewModel.Id, viewModel.Currency.Value)).AddTo(viewModel);
            
            var view = _shopPanelView.GetMinerShopView(viewModel);
            _gameplayViewStorage.AddMinerShopView(data.Id, view);
        }
        
        private void UpdateMinerShop(UpdateMinerShopData data)
        {
            var viewModel = _minerShopItems[data.Id];
            viewModel.SetPrice(data.Currency, data.Price);
        }
    }
}