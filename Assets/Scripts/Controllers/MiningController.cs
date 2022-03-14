using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningController : MonoBehaviour, ISavable
{
    public const float multForAbseceReward = 3;

    private Player player;
    private MergeFieldBuilder mergeFieldBuilder;

    public float Boost { get; set; } = 1;

    private DateTime lastActiveDateTime;

    public void Construct(Player player, MergeFieldBuilder mergeFieldBuilder)
    {
        this.player = player;
        this.mergeFieldBuilder = mergeFieldBuilder;
    }

    public void Update()
    {
        AddCoinsForPeriod(Time.deltaTime);
    }

    public void AddCoinsForPeriod(float periodTimeInSeconds)
    {
        foreach (var cell in mergeFieldBuilder.Cells)
        {
            if (cell.IsMiningCell && !cell.IsEmpty)
            {
                player.AddCoins(cell.MiningDevice.Data.CoinsPerSecond * periodTimeInSeconds * Boost);
            }
        }
    }

    public double GetCoinsForPeriod(float periodTimeInSeconds)
    {
        double coins = 0;

        foreach (var cell in mergeFieldBuilder.Cells)
        {
            if (cell.IsMiningCell && !cell.IsEmpty)
            {
                coins += cell.MiningDevice.Data.CoinsPerSecond * periodTimeInSeconds * Boost;
            }
        }

        return coins;
    }

    public void AddCoinsForAbsenceTime(float mult = 1)
    {
        AddCoinsForPeriod(GetAbsenceTimeSeconds() * mult);
        Debug.Log($"added coins for {GetAbsenceTimeSeconds()} sec * {mult}");
    }
    public double GetCoinsForAbsenceTime(float mult = 1)
    {
        return GetCoinsForPeriod(GetAbsenceTimeSeconds() * mult);
    }

    public float GetAbsenceTimeSeconds()
    {
        var timePast = DateTime.Now - lastActiveDateTime;
        var secPast = timePast.TotalSeconds;
        var floatSecPast = (float)secPast;
        return floatSecPast;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            Load();
            AddCoinsForAbsenceTime();
        }
    }

    public void Save()
    {
        Saver.Save(SaveNames.LastActiveDateTime, new DateTimeSave(DateTime.Now.ToString()));
    }
    public void Load()
    {
        var dateStr = Saver.Load(SaveNames.LastActiveDateTime, new DateTimeSave(DateTime.Now.ToString())).DateTimeStr;
        lastActiveDateTime = DateTime.Parse(dateStr);
    }
}

[Serializable]
public class DateTimeSave
{
    [SerializeField]
    public string DateTimeStr;

    public DateTimeSave(string DateTimeStr)
    {
        this.DateTimeStr = DateTimeStr;
    }
}
