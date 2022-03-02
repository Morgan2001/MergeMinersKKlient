using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopPosition : MonoBehaviour
{
    public Image Image;
    public Text DescriptionText;
    public Text CostText;
    public Button BuyButton;

    public void Construct(BoostController boostController, Player player, UpgradeOffer upgradeOffer, Ads ads)
    {
        Image.sprite = upgradeOffer.Sprite;
        DescriptionText.text = upgradeOffer.Description;

        if (upgradeOffer.Currency == ResourceTypes.Diamonds)
        {
            CostText.text = upgradeOffer.Cost.ToString();

            BuyButton.onClick.AddListener(() =>
            {
                if (player.Diamonds >= upgradeOffer.Cost)
                {
                    boostController.Boost(upgradeOffer.BoostType, upgradeOffer.Power, upgradeOffer.Duration);
                    player.Diamonds -= upgradeOffer.Cost;
                }
            });
        }
        else
        {
            BuyButton.onClick.AddListener(() =>
            {
                ads.ShowRewarded(() => boostController.Boost(upgradeOffer.BoostType, upgradeOffer.Power, upgradeOffer.Duration));
            });
        }
        
    }
}
