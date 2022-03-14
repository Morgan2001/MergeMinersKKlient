using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnWindow : MonoBehaviour
{
    public Text idleSum;
    public Text multSum;
    public Text getIdleSum;

    public string getIdleSumText;

    public void SetData(MiningController miningController)
    {
        idleSum.text = LargeNumberFormatter.FormatNumber(miningController.GetCoinsForAbsenceTime());
        multSum.text = LargeNumberFormatter.FormatNumber(miningController.GetCoinsForAbsenceTime
            (MiningController.multForAbseceReward));
        getIdleSum.text = getIdleSumText + LargeNumberFormatter.FormatNumber(miningController.GetCoinsForAbsenceTime());
    }
}
