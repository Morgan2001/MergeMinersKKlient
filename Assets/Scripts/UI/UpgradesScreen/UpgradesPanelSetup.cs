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
        private IResourceHelper _resourceHelper;
        
        [Inject]
        private void Setup(
            UpgradesConnector upgradesConnector,
            IResourceHelper resourceHelper)
        {
            _upgradesConnector = upgradesConnector;
            _resourceHelper = resourceHelper;

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
            var description = _resourceHelper.GetUpgradeDescription(index);
            var viewModel = new UpgradeViewModel(data.Id, icon, description, data.Price);

            var prefab = data.Currency == Currency.Ads ? _upgradePrefabAds : _upgradePrefabGems;
            var view = Instantiate(prefab, _container);
            view.Bind(viewModel);

            view.BuyEvent.Subscribe(_upgradesConnector.Buy).AddTo(view);
        }
    }
}