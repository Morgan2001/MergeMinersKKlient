using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LargeNumberFormatter
{
    public static Dictionary<int, string> abbreviatedNotationsOfDegrees = new Dictionary<int, string>()
    {
        [3]= "K",
        [6]= "M",
        [9]= "B",
        [12]= "T",
        [15]= "q",
        [18]= "Q",
        [21]= "s",
        [24]= "S",
        [27]= "O",
        [30]= "N",
        [33]= "d",
        [36]= "U",
        [39]= "D"
    };

    public static string FormatNumber(double num)
    {
        var power = (int)Math.Log10(num);

        if (power < 3)
        {
            return ((int)num).ToString();
        }
        else
        {
            var numRemainder = num / Math.Pow(10, power);
            var powerMain = power / 3;
            powerMain *= 3;
            var powerRemainder = power % 3;

            numRemainder *= powerRemainder == 0 ? 1 : Math.Pow(10, powerRemainder);

            return numRemainder.ToString("F2") + abbreviatedNotationsOfDegrees[powerMain];
        }
    }
}
