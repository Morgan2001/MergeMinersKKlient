using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    public bool IsOn { get; private set; } = true;

    public void Construct()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        if (!IsOn)
        {
            var image = GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;

            IsOn = true;
        }
    }
    public void TurnOff()
    {
        if (IsOn)
        {
            var image = GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;

            IsOn = false;
        }
    }
}
