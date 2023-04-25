using MergeMiner.Core.Network.Data;
using MergeMiner.Core.Network.Helpers;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;

namespace GameCore.Services
{
    public class GameStateApplier
    {
        private readonly PlayerRepository _playerRepository;
        private readonly PlayerMinersRepository _playerMinersRepository;
        private readonly PlayerSlotsRepository _playerSlotsRepository;
        private readonly PlayerBoxRepository _playerBoxRepository;
        private readonly PlayerShopRepository _playerShopRepository;
        private readonly PlayerBonusesRepository _playerBonusesRepository;
        private readonly PlayerFreeGemRepository _playerFreeGemRepository;
        private readonly PlayerMissionsRepository _playerMissionsRepository;
        private readonly PlayerSubscriptionRepository _playerSubscriptionRepository;
        private readonly MinerPoolService _minerPoolService;
        private readonly BonusPoolService _bonusPoolService;

        public GameStateApplier(
            PlayerRepository playerRepository,
            PlayerMinersRepository playerMinersRepository,
            PlayerSlotsRepository playerSlotsRepository,
            PlayerBoxRepository playerBoxRepository,
            PlayerShopRepository playerShopRepository,
            PlayerBonusesRepository playerBonusesRepository,
            PlayerFreeGemRepository playerFreeGemRepository,
            PlayerMissionsRepository playerMissionsRepository,
            PlayerSubscriptionRepository playerSubscriptionRepository,
            MinerPoolService minerPoolService,
            BonusPoolService bonusPoolService)
        {
            _playerRepository = playerRepository;
            _playerMinersRepository = playerMinersRepository;
            _playerSlotsRepository = playerSlotsRepository;
            _playerBoxRepository = playerBoxRepository;
            _playerShopRepository = playerShopRepository;
            _playerBonusesRepository = playerBonusesRepository;
            _playerFreeGemRepository = playerFreeGemRepository;
            _playerMissionsRepository = playerMissionsRepository;
            _playerSubscriptionRepository = playerSubscriptionRepository;
            _minerPoolService = minerPoolService;
            _bonusPoolService = bonusPoolService;
        }

        public void Apply(GameState gameState, string playerId)
        {
            _playerRepository.Get(playerId).Apply(gameState.Player);
            _playerMinersRepository.Get(playerId).Apply(gameState.Miners, _minerPoolService);
            _playerSlotsRepository.Get(playerId).Apply(gameState.Slots);
            _playerBoxRepository.Get(playerId).Apply(gameState.Box);
            _playerShopRepository.Get(playerId).Apply(gameState.Shop);
            _playerBonusesRepository.Get(playerId).Apply(gameState.Bonuses, _bonusPoolService);
            _playerFreeGemRepository.Get(playerId).Apply(gameState.FreeGem);
            _playerMissionsRepository.Get(playerId).Apply(gameState.Missions);
            _playerSubscriptionRepository.Get(playerId).Apply(gameState.Subscription);
        }
    }
}