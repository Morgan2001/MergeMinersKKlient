using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseHandler : MonoBehaviour
{
    public void HandlePurchase(Product product)
    {
        switch (product.definition.id)
        {
            case "com.mogames.get.crypto.nft.game.diamonds150":
                break;
            case "com.mogames.get.crypto.nft.game.diamonds340":
                break;
            case "com.mogames.get.crypto.nft.game.diamonds950":
                break;
            case "com.mogames.get.crypto.nft.game.diamonds2250":
                break;
        }
    }
}
