using System.Collections.Generic;
using GameCore.Connectors;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.GameplayPanel.ShopPanel
{
    public class MinerShopPanelSetup : MonoBehaviour
    {
        [SerializeField] private ShopPanelView _shopPanelView;

        private MinerShopConnector _minerShopConnector;
        private IResourceHelper _resourceHelper;
        private GameplayViewStorage _gameplayViewStorage;
        
        private ShopPanelViewModel _shopPanelViewModel;

        private Dictionary<string, MinerShopViewModel> _minerShopItems = new();

        [Inject]
        private void Setup(
            MinerShopConnector minerShopConnector,
            IResourceHelper resourceHelper,
            GameplayViewStorage gameplayViewStorage)
        {
            _minerShopConnector = minerShopConnector;
            _resourceHelper = resourceHelper;
            _gameplayViewStorage = gameplayViewStorage;
            
            _shopPanelViewModel = new ShopPanelViewModel();
            _shopPanelView.Bind(_shopPanelViewModel);

            _minerShopConnector.AddMinerShopEvent.Subscribe(AddMinerShop).AddTo(_shopPanelView);
            _minerShopConnector.UpdateMinerShopEvent.Subscribe(UpdateMinerShop).AddTo(_shopPanelView);
        }

        private void AddMinerShop(AddMinerShopData data)
        {
            var icon = _resourceHelper.GetNormalIconByLevel(data.Level);
            var viewModel = new MinerShopViewModel(data.Id, data.Level, icon);
            _shopPanelViewModel.AddMiner(viewModel);
            _minerShopItems.Add(data.Id, viewModel);
            
            viewModel.ClickEvent.Subscribe(() => _minerShopConnector.BuyMiner(viewModel.Level, viewModel.Currency.Value)).AddTo(viewModel);
            
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