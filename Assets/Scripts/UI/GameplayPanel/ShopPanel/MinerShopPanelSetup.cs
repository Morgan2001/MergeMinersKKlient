using System.Collections.Generic;
using GameCore.Connectors;
using MergeMiner.Core.State.Data;
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
        private AdsConnector _adsConnector;
        
        private ShopPanelViewModel _shopPanelViewModel;

        private Dictionary<string, MinerShopViewModel> _minerShopItems = new();

        [Inject]
        private void Setup(
            MinerShopConnector minerShopConnector,
            IResourceHelper resourceHelper,
            GameplayViewStorage gameplayViewStorage,
            AdsConnector adsConnector)
        {
            _minerShopConnector = minerShopConnector;
            _resourceHelper = resourceHelper;
            _gameplayViewStorage = gameplayViewStorage;
            _adsConnector = adsConnector;
            
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
            
            viewModel.ClickEvent.Subscribe(() =>
            {
                if (viewModel.Currency.Value == Currency.Ads)
                {
                    _adsConnector.ShowRewarded(x =>
                    {
                        if (x) BuyMiner(viewModel.Level, viewModel.Currency.Value);
                    });
                }
                else
                {
                    BuyMiner(viewModel.Level, viewModel.Currency.Value);
                }
            }).AddTo(viewModel);
            
            var view = _shopPanelView.GetMinerShopView(viewModel);
            _gameplayViewStorage.AddMinerShopView(data.Id, view);
        }

        private void BuyMiner(int level, Currency currency)
        {
            _minerShopConnector.BuyMiner(level, currency);
        }
        
        private void UpdateMinerShop(UpdateMinerShopData data)
        {
            var viewModel = _minerShopItems[data.Id];
            viewModel.SetPrice(data.Currency, data.Price);
        }
    }
}