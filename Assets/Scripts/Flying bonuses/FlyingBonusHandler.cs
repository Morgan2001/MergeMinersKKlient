using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBonusHandler
{
    public const int CoinsForWallet = 1000;

    private Player player;
    private WindowController windowController;

    public FlyingBonusHandler(Player player, WindowController windowController)
    {
        this.player = player;
        this.windowController = windowController;
    }

    public void HandleFlyingBonus(FlyingBonuses type)
    {
        switch (type)
        {
            case FlyingBonuses.Wallet:
                player.AddCoins(CoinsForWallet);
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
