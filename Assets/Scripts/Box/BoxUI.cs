using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    public Image FillImage;

    public void UpdateFillAmount(float fillAmount)
    {
        FillImage.fillAmount = fillAmount;
    }
}
