using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Relocator : MonoBehaviour
{
    public Image LocationImage;

    private MergeFieldBuilder mergeFieldBuilder;
    private MergeFieldFiller mergeFieldFiller;
    private Player player;
    private GameRules gameRules;
    private FillIndicatorUI relocationIndicator;

    private int[] oldCells;
    private Locations prevLocation;

    private const int NullIntValue = -1;

    private ButtonHighlight buttonHighlight;

    public void Construct(MergeFieldBuilder mergeFieldBuilder, MergeFieldFiller mergeFieldFiller, Player player, GameRules gameRules, 
        FillIndicatorUI relocationIndicator, ButtonHighlight buttonHighlight)
    {
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.mergeFieldFiller = mergeFieldFiller;
        this.player = player;
        this.gameRules = gameRules;
        this.relocationIndicator = relocationIndicator;
        this.buttonHighlight = buttonHighlight;

        LocationImage.sprite = gameRules.Locations[player.CurLocation].Sprite;

        player.MaxAchivedMiningDeviceLevelUpdated += UpdateRelocationIndicator;
        UpdateRelocationIndicator();
    }

    private void UpdateRelocationIndicator()
    {
        var curLocRelocateLevel = gameRules.Locations[player.CurLocation].MinMiningDeviceLevelToRelocate;
        var percentToRelocation = (float)(player.MaxAchivedMiningDeviceLevel - curLocRelocateLevel) / 
            (float)(GetNewLocation().MinMiningDeviceLevelToRelocate - curLocRelocateLevel);
        relocationIndicator.UpdateFillAmount(percentToRelocation <= 1 ? percentToRelocation : 1);
        UpdateHighlight(percentToRelocation);
    }

    private void UpdateHighlight(float percentToRelocation)
    {
        if (percentToRelocation >= 1)
        {
            buttonHighlight.TurnOn();
        }
        else
        {
            buttonHighlight.TurnOff();
        }
    }

    public void RelocateToNext()
    {
        LocationData newLoc = GetNewLocation();
        if (newLoc != null)
        {
            Relocate(newLoc.Type);
            UpdateRelocationIndicator();
        }
    }

    private LocationData GetNewLocation()
    {
        return gameRules.Locations.LocationDatas.Find(location => location.Level == gameRules.Locations[player.CurLocation].Level + 1);
    }

    public void Relocate(Locations location)
    {
        prevLocation = player.CurLocation;
        player.UpdateLocation(location);
        LocationImage.sprite = gameRules.Locations[location].Sprite;

        SaveOldCellsIds();
        UpgradeOldCellsArrayAccordingToNewField();
        mergeFieldBuilder.BuildMergeField();
        mergeFieldFiller.FillFieldFromIntList(new List<int>(oldCells));
    }

    private void UpgradeOldCellsArrayAccordingToNewField()
    {
        var prevLocationSettings = gameRules.Locations[prevLocation];
        var newLocationSettings = gameRules.Locations[player.CurLocation];

        var upgradedCells = new int[newLocationSettings.NumOfRows * newLocationSettings.NumOfColumns];

        var oldCount = 0;
        var newCount = 0;

        for (int i = 0; i < newLocationSettings.NumOfRows; i++)
        {
            for (int j = 0; j < newLocationSettings.NumOfColumns; j++)
            {
                if (i < prevLocationSettings.NumOfRows && j < prevLocationSettings.NumOfColumns)
                {
                    upgradedCells[newCount++] = oldCells[oldCount++];
                }
                else
                {
                    upgradedCells[newCount++] = NullIntValue;
                }
            }
        }

        oldCells = upgradedCells;
    }

    private void SaveOldCellsIds()
    {
        oldCells = new int[mergeFieldBuilder.Cells.Count];
        
        for (int i = 0; i < mergeFieldBuilder.Cells.Count; i++)
        {
            if (mergeFieldBuilder.Cells[i].MiningDevice != null)
            {
                oldCells[i] = mergeFieldBuilder.Cells[i].MiningDevice.Data.id;
            }
            else
            {
                oldCells[i] = NullIntValue;
            }
        }
    }
}
