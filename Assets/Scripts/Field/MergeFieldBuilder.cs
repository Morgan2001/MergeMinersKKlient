using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeFieldBuilder : MonoBehaviour
{
    public GameObject MergePanel;
    public GameObject CellPrefab;
    public GameObject MiningCellBackPrefab;

    private P.Player player;
    private MiningDeviceMerger miningDeviceMerger;
    private GameRules gameRules;
    private Subscription subscription;
    public LocationData CurLocSettings { get; private set; }

    public List<Cell> Cells { get; private set; }
    public float CellSize { get; private set; }

    public void Construct(P.Player player, MiningDeviceMerger miningDeviceMerger, GameRules gameRules, Subscription subscription)
    {
        this.player = player;
        this.miningDeviceMerger = miningDeviceMerger;
        this.gameRules = gameRules;
        this.subscription = subscription;
        subscription.InitComplete += UpdateMiningSlots;
    }

    private float CalcCellSize()
    {
        var curLocSettings = gameRules.Locations[player.CurLocation];
        var spacing = MergePanel.GetComponent<GridLayoutGroup>().spacing.x;

        var cellSize = MergePanel.GetComponent<RectTransform>().rect.width / curLocSettings.NumOfColumns - spacing;

        var mergeFieldHeight = MergePanel.GetComponent<RectTransform>().rect.height;

        if ((cellSize + spacing) * curLocSettings.NumOfRows > mergeFieldHeight)
        {
            cellSize = mergeFieldHeight / curLocSettings.NumOfRows - spacing;
        }

        return cellSize;
    }

    public bool HasEmptyCells()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            if (Cells[i].IsEmpty && !Cells[i].IsBusyByAnimation)
            {
                return true;
            }
        }

        return false;
    }

    public void BuildMergeField()
    {
        UpdateCurLocSettings();
        CellSize = CalcCellSize();
        
        DestroyAllCells();
        Cells = new List<Cell>();

        SetGridLayoutGroupSettings();
        AddCells();
    }

    private void DestroyAllCells()
    {
        if (Cells != null)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                Destroy(Cells[i].gameObject);
            }
        }
    }

    private void AddCells()
    {
        for (int i = 0; i < CurLocSettings.NumOfRows * CurLocSettings.NumOfColumns; i++)
        {
            Cells.Add(CreateCell(i));
        }
    }

    private Cell CreateCell(int numOfCurCell)
    {
        var cellGO = Instantiate(CellPrefab);
        cellGO.transform.SetParent(MergePanel.transform);
        cellGO.transform.localScale = Vector3.one;
        var cell = cellGO.GetComponent<Cell>();

        var subExtraCells = 0;
        if (subscription.IsActive())
        {
            subExtraCells = subscription.Data.MiningSlots;
        }
        
        if (numOfCurCell < CurLocSettings.NumOfMiningCells + subExtraCells)
        {
            cell.Construct(miningDeviceMerger, true);
        }
        else
        {
            cell.Construct(miningDeviceMerger, false);
        }

        return cell;
    }

    private void SetGridLayoutGroupSettings()
    {
        var glg = MergePanel.GetComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(CellSize, CellSize);
        glg.constraintCount = CurLocSettings.NumOfColumns;
    }

    private void UpdateCurLocSettings()
    {
        CurLocSettings = gameRules.Locations[player.CurLocation];
    }

    public void UpdateMiningSlots()
    {
        var subExtraCells = 0;
        if (subscription.IsActive())
        {
            subExtraCells = subscription.Data.MiningSlots;
        }

        for (int i = 0; i < Cells.Count; i++)
        {
            if (i < CurLocSettings.NumOfMiningCells + subExtraCells)
            {
                if (!Cells[i].IsMiningCell)
                {
                    Cells[i].BecomeMiningCell();
                }
            }
        }
    }
}
