using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelAttemptsUI : MonoBehaviour
{
    private Wheel wheel;
    private Text text;

    public void Construct(Wheel wheel)
    {
        this.wheel = wheel;
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = $"{wheel.AttemptsForDayLeft}/{wheel.AttemptsForDay}";
    }
}
