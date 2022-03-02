using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillIndicatorUI : MonoBehaviour
{
    public void UpdateFillAmount(float fillAmount)
    {
        GetComponent<Image>().fillAmount = fillAmount;
    }
}
