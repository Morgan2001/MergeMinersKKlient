using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseHandler : MonoBehaviour
{
    private Player player;

    public void Construct(Player player)
    {
        this.player = player;
    }

    public void HandlePurchase(Product product)
    {
        switch (product.definition.id)
        {
            case "com.mogames.get.crypto.nft.game.diamonds150":
                player.AddDiamonds(150);
                break;
            case "com.mogames.get.crypto.nft.game.diamonds340":
                player.AddDiamonds(340);
                break;
            case "com.mogames.get.crypto.nft.game.diamonds950":
                player.AddDiamonds(950);
                break;
            case "com.mogames.get.crypto.nft.game.diamonds2250":
                player.AddDiamonds(2250);
                break;
        }
    }
}
