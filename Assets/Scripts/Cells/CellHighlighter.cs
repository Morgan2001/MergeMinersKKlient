using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlighter
{
    private MergeFieldBuilder mergeFieldBuilder;

    public CellHighlighter(MergeFieldBuilder mergeFieldBuilder)
    {
        this.mergeFieldBuilder = mergeFieldBuilder;
    }

    public void HighlightAllPossibleMerges(Cell fromCell)
    {
        foreach (var cell in mergeFieldBuilder.Cells)
        {
            if (!cell.IsEmpty && cell.MiningDevice.Data.Type == fromCell.MiningDevice.Data.Type && cell != fromCell)
            {
                cell.Highlight();
            }
        }
    }
    public void RemoveHighlight()
    {
        foreach (var cell in mergeFieldBuilder.Cells)
        {
            cell.RemoveHighlight();
        }
    }
}
