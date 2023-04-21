using System.Collections;
using System.Collections.Generic;
using MergeMiner.Core.State.Config;
using UnityEngine;

public class FlyingBonusHandler
{
    public P.Player player;
    private WindowController windowController;
    public GameRules gameRules;

    public FlyingBonusHandler(P.Player player, WindowController windowController, GameRules gameRules)
    {
        this.player = player;
        this.windowController = windowController;
        this.gameRules = gameRules;
    }

    // public void HandleFlyingBonus(BonusType type)
    // {
        // switch (type)
        // {
            // case BonusType.Money:
                // player.AddCoins(gameRules.WalletRewards.GetRewardByLevel(player.MaxAchivedMiningDeviceLevel));
                // break;
            // case BonusType.Chip:
                // windowController.ShowChipBonusWindow();
                // break;
            // case BonusType.Power:
                // windowController.ShowPowerTransformerBonusWindow();
                // break;
            // case BonusType.Flash:
                // windowController.ShowFlashCardBonusWindow();
                // break;
            // case BonusType.Miners:
                // windowController.ShowBoxWithMinersBonusWindow();
                // break;
        // }
    // }
}
