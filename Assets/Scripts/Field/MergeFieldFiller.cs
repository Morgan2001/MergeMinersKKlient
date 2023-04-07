using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MergeMiner.Core.State.Enums;
using UnityEngine;

public class MergeFieldFiller : ISavable
{
    private MergeFieldBuilder mergeFieldBuilder;
    private MiningDevicePlacer miningDevicePlacer;
    private MiningDeviceGenerator miningDeviceGenerator;
    private GameRules gameRules;
    private MiningDeviceChooser miningDeviceChooser;

    public MergeFieldFiller(MergeFieldBuilder mergeFieldBuilder, MiningDevicePlacer miningDevicePlacer, 
        MiningDeviceGenerator miningDeviceGenerator, GameRules gameRules, MiningDeviceChooser miningDeviceChooser)
    {
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.miningDevicePlacer = miningDevicePlacer;
        this.miningDeviceGenerator = miningDeviceGenerator;
        this.gameRules = gameRules;
        this.miningDeviceChooser = miningDeviceChooser;

        Load();
    }

    public void AddDeviceFromBox(MiningDevices deviceType, MinerSource boxType, Transform boxTransform)
    {
        if (mergeFieldBuilder.HasEmptyCells())
        {
            miningDevicePlacer.SetParameters(PlaceTypes.ToRandomPossible, true, boxTransform);
            miningDevicePlacer.Place(miningDeviceGenerator.CreateMiningDevice(deviceType, true, boxType));
        }
    }

    public void AddDeviceFromShop(MiningDevices deviceType, Transform from)
    {
        if (mergeFieldBuilder.HasEmptyCells())
        {
            miningDevicePlacer.SetParameters(PlaceTypes.ToRandomPossible, true, from);
            miningDevicePlacer.Place(miningDeviceGenerator.CreateMiningDevice(deviceType, true, MinerSource.Shop));
        }
    }

    public void AddDevicesFromBoxWithMiners()
    {
        for (int i = 0; i < 4; i++)
        {
            if (mergeFieldBuilder.HasEmptyCells())
            {
                var deviceType = miningDeviceChooser.ChooseRandomMiningDeviceFromBoxToMaxAchived();
                miningDevicePlacer.SetParameters(PlaceTypes.ToRandomPossible);
                miningDevicePlacer.Place(miningDeviceGenerator.CreateMiningDevice(deviceType, true, MinerSource.Random));
            }
            else
            {
                return;
            }
        }
    }

    public void FillFieldFromIntList(List<int> ids)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            var data = gameRules.MiningDevices.MiningDeviceDatas.Find(d => d.id == ids[i]);

            if (data != null)
            {
                miningDevicePlacer.PlaceTo(miningDeviceGenerator.CreateMiningDevice(data.Type, false), i);
            }
        }
    }

    public void Save()
    {
        var cells = mergeFieldBuilder.Cells;
        var cellIds = new List<int>();

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].MiningDevice != null)
            {
                cellIds.Add(cells[i].MiningDevice.Data.id);
            }
            else
            {
                cellIds.Add(int.MinValue);
            }
        }

        Saver.Save(SaveNames.GameplayField, new CellIdsSave() { ids = cellIds });
    }
    public void Load()
    {
        var defaultIds = Enumerable.Repeat(int.MinValue, 25).ToList();

        var ids = Saver.Load(SaveNames.GameplayField, new CellIdsSave() { ids = defaultIds }).ids;

        FillFieldFromIntList(ids);
    }
}

[Serializable]
public class CellIdsSave
{
    [SerializeField]
    public List<int> ids;
}