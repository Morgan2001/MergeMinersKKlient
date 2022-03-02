using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopDiamondOfferPosition
{
    public ShopDiamondPosition ShopDiamondPosition;
    public ShopDiamondOffer ShopDiamondOffer;
}

public class Shop : MonoBehaviour
{
    public List<ShopDiamondOfferPosition> ShopDiamondOfferPositions;

    public void Construct()
    {
        foreach (var offerPos in ShopDiamondOfferPositions)
        {
            offerPos.ShopDiamondPosition.SetOffer(offerPos.ShopDiamondOffer.DiamondNum, offerPos.ShopDiamondOffer.Price);
        }
    }
}
