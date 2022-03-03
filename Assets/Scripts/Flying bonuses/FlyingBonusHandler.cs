using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBonusHandler
{
    public Player player;
    private WindowController windowController;
    public GameRules gameRules;

    public FlyingBonusHandler(Player player, WindowController windowController, GameRules gameRules)
    {
        this.player = player;
        this.windowController = windowController;
        this.gameRules = gameRules;
    }

    public void HandleFlyingBonus(FlyingBonuses type)
    {
        switch (type)
        {
            case FlyingBonuses.Wallet:
                player.AddCoins(gameRules.WalletRewards.GetRewardByLevel(player.MaxAchivedMiningDeviceLevel));
                break;
            case FlyingBonuses.Chip:
                windowController.ShowChipBonusWindow();
                break;
            case FlyingBonuses.PowerTransformer:
                windowController.ShowPowerTransformerBonusWindow();
                break;
            case FlyingBonuses.FlashCard:
                windowController.ShowFlashCardBonusWindow();
                break;
            case FlyingBonuses.BoxWithMiners:
                windowController.ShowBoxWithMinersBonusWindow();
                break;
        }
    }
}
