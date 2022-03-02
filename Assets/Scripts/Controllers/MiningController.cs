using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningController : MonoBehaviour, ISavable
{
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

    public void AddCoinsForAbsenceTime()
    {
        var timeHasPast = DateTime.Now - lastActiveDateTime;
        AddCoinsForPeriod((float)timeHasPast.TotalSeconds);
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
