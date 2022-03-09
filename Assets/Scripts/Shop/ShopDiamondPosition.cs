using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDiamondPosition : MonoBehaviour
{
    public Text NumOfDiamonds;
    public Text Price;

    public void SetOffer(int numOfDiamonds, float price)
    {
        NumOfDiamonds.text = numOfDiamonds.ToString();
        Price.text = price.ToString() + " $";
    }
}
