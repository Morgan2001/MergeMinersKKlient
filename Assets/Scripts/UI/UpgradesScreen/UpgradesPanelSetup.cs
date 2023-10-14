using GameCore.Connectors;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.UpgradesScreen
{
    public class UpgradesPanelSetup : MonoBehaviour
    {
        [SerializeField] private UpgradeView _upgradePrefabGems;
        [SerializeField] private UpgradeView _upgradePrefabAds;
        [SerializeField] private Transform _container;

        private UpgradesConnector _upgradesConnector;
        private AdsConnector _adsConnector;
        private IResourceHelper _resourceHelper;
        private TabSwitcher _tabSwitcher;
        
        [Inject]
        private void Setup(
            UpgradesConnector upgradesConnector,
            AdsConnector adsConnector,
            IResourceHelper resourceHelper,
            TabSwitcher tabSwitcher)
        {
            _upgradesConnector = upgradesConnector;
            _adsConnector = adsConnector;
            _resourceHelper = resourceHelper;
            _tabSwitcher = tabSwitcher;

            var upgrades = _upgradesConnector.GetUpgrades();
            var index = 0;
            foreach (var item in upgrades)
            {
                AddUpgrade(index++, item);
            }
        }

        private void AddUpgrade(int index, UpgradesConfigItem data)
        {
            var icon = _resourceHelper.GetUpgradeIcon(index);
            var viewModel = new UpgradeViewModel(data.Id, icon, data.Price);

            var prefab = data.Currency == Currency.Ads ? _upgradePrefabAds : _upgradePrefabGems;
            var view = Instantiate(prefab, _container);
            view.Bind(viewModel);

            view.BuyEvent.Subscribe(id =>
            {
                if (data.Currency == Currency.Ads)
                {
                    _adsConnector.ShowRewarded(x =>
                    {
                        if (x)
                        {
                            Buy(id);
                        }
                    });
                }
                else if (_upgradesConnector.CanBuy(data.Currency, data.Price))
                {
                    Buy(id);
                }
                else
                {
                    _tabSwitcher.SwitchTab(Tab.Shop);
                }
            }).AddTo(view);
        }

        private void Buy(string id)
        {
            _upgradesConnector.Buy(id);
        }
    }
}