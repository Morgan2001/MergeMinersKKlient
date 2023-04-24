using System.Collections.Generic;
using GameCore.Services;
using MergeMiner.Core.State.Config;

namespace GameCore.Connectors
{
    public class UpgradesConnector
    {
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly UpgradesConfig _upgradesConfig;

        public UpgradesConnector(
            PlayerActionProxy playerActionProxy,
            UpgradesConfig upgradesConfig)
        {
            _playerActionProxy = playerActionProxy;
            _upgradesConfig = upgradesConfig;
        }

        public IEnumerable<UpgradesConfigItem> GetUpgrades()
        {
            return _upgradesConfig.GetAll();
        }

        public void Buy(string id)
        {
            _playerActionProxy.BuyUpgrade(id);
        }
    }
}