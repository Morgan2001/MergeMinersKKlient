using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum SpinPriceTypes
{
    ForAd,
    ForDiamonds
}


public class Wheel : MonoBehaviour, ISavable
{
    public List<WheelReward> PossibleRewards;
    public int NumOfTurns;
    public int AttemptsForDay;

    [Header("UI")]
    public GameObject ButtonPanel;

    [SerializeField]
    private DateTime lastDate;
    [SerializeField] [HideInInspector]
    private int attemptsForDayLeft;

    private BoostController boostController;
    private WindowController windowController;


    public int AttemptsForDayLeft { get => attemptsForDayLeft; }

    public void Construct(BoostController boostController, WindowController windowController)
    {
        this.boostController = boostController;
        this.windowController = windowController;

        Load();
    }

    public void Spin(SpinPriceTypes type)
    {
        if (type == SpinPriceTypes.ForAd && attemptsForDayLeft <= 0)
        {
            return;
        }

        var rewardNum = UnityEngine.Random.Range(0, PossibleRewards.Count);
        var reversedNum = PossibleRewards.Count - rewardNum;
        var rewardDegrees = 360 / (PossibleRewards.Count) * reversedNum;

        var seq = DOTween.Sequence();

        seq.Append(transform.DORotate(new Vector3(0, 0, -(360 * NumOfTurns + rewardDegrees)), 5, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic));
        seq.AppendInterval(1);

        ButtonPanel.SetActive(false);

        seq.OnComplete(() =>
        {
            windowController.ShowWheelRewardWindow(PossibleRewards[rewardNum].Sprite, PossibleRewards[rewardNum].Description);
            boostController.Boost(PossibleRewards[rewardNum].BoostType, PossibleRewards[rewardNum].Power, PossibleRewards[rewardNum].Duration);
            SetToDefault();

            if (type == SpinPriceTypes.ForAd)
            {
                attemptsForDayLeft -= 1;
            }
            ButtonPanel.SetActive(true);
        });
    }

    public void SetToDefault()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Save()
    {
        Saver.Save(SaveNames.Wheel, new WheelSave(lastDate.ToString(), AttemptsForDayLeft));
    }
    public void Load()
    {
        var wheelSave = Saver.Load(SaveNames.Wheel, new WheelSave(DateTime.Now.ToString(), AttemptsForDay));


        lastDate = DateTime.Parse(wheelSave.Date);

        if (lastDate.Date == DateTime.Now.Date)
        {
            attemptsForDayLeft = wheelSave.AttemptsLeft;
        }
        else
        {
            lastDate = DateTime.Now;
            attemptsForDayLeft = AttemptsForDay;
        }
    }
}

[Serializable]
public class WheelSave
{
    [SerializeField]
    private string date;
    public string Date => date;
    [SerializeField]
    private int attemptsLeft;
    public int AttemptsLeft => attemptsLeft;

    public WheelSave(string date, int attemptsLeft)
    {
        this.date = date;
        this.attemptsLeft = attemptsLeft;
    }
}
