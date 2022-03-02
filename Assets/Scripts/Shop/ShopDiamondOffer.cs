using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDiamondOffer", menuName = "ScriptableObjects/ShopDiamondOffer", order = 1)]
public class ShopDiamondOffer : ScriptableObject
{
    public int id;
    public int DiamondNum;
    public float Price;
}
