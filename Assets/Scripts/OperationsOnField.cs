using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OperationsOnField
{
    private MergeFieldBuilder mergeFieldBuilder;

    public OperationsOnField(MergeFieldBuilder mergeFieldBuilder)
    {
        this.mergeFieldBuilder = mergeFieldBuilder;
    }

    public (Cell, Cell) FindCellsWithIdenticalMiningDevices()
    {
        var cells = new List<Cell>(mergeFieldBuilder.Cells);
        cells.RemoveAll(x => x.IsEmpty || x.MiningDevice.GetComponent<MiningDeviceBox>().IsInBox);

        if (cells.Count < 2)
        {
            return (null, null);
        }

        var sortedCells = cells.OrderBy(x => x.MiningDevice.Data.Type).ToList();

        var prevCell = sortedCells[0];

        for (int i = 1; i < sortedCells.Count; i++)
        {
            var curCell = sortedCells[i];
            
            if (curCell.MiningDevice.Data.Type == prevCell.MiningDevice.Data.Type)
            {
                return (curCell, prevCell);
            }

            prevCell = curCell;
        }

        return (null, null);
    }

    public Cell FindCellWithUnopenedBox()
    {
        for (int i = 0; i < mergeFieldBuilder.Cells.Count; i++)
        {
            if (!mergeFieldBuilder.Cells[i].IsEmpty && mergeFieldBuilder.Cells[i].MiningDevice.GetComponent<MiningDeviceBox>().IsInBox)
            {
                return mergeFieldBuilder.Cells[i];
            }
        }

        return null;
    }

    public Cell FindEmptyMiningSlot()
    {
        return mergeFieldBuilder.Cells.Find(cell => cell.IsEmpty && cell.IsMiningCell);
    }
    public Cell FindNotMiningCellWithMiningDeviceNotInBox()
    {
        return mergeFieldBuilder.Cells.Find(cell => !cell.IsMiningCell && !cell.IsEmpty && !cell.MiningDevice.GetComponent<MiningDeviceBox>().IsInBox);
    }
}
