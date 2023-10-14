using System.Collections.Generic;
using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Repository;

namespace GameCore.Connectors
{
    public class UpgradesConnector
    {
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly UpgradesConfig _upgradesConfig;
        private readonly SessionData _sessionData;
        private readonly PlayerRepository _playerRepository;

        public UpgradesConnector(
            PlayerActionProxy playerActionProxy,
            UpgradesConfig upgradesConfig,
            SessionData sessionData,
            PlayerRepository playerRepository)
        {
            _playerActionProxy = playerActionProxy;
            _upgradesConfig = upgradesConfig;
            _sessionData = sessionData;
            _playerRepository = playerRepository;
        }

        public IEnumerable<UpgradesConfigItem> GetUpgrades()
        {
            return _upgradesConfig.GetAll();
        }

        public bool CanBuy(Currency currency, double price)
        {
            var player = _playerRepository.Get(_sessionData.Token);
            switch (currency)
            {
                case Currency.Money: return player.Money >= price;
                case Currency.Gems: return player.Gems >= price;
                case Currency.Ads: return true;
            }
            return false;
        }

        public void Buy(string id)
        {
            _playerActionProxy.BuyUpgrade(id);
        }
    }
}