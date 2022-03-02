using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceTypes
{
    ToRandomPossible
}

public class MiningDevicePlacer : MonoBehaviour
{
    PlaceTypes placeType;
    bool withJumpAnimation; 
    Transform objectToJumpFrom;

    MergeFieldBuilder mergeFieldBuilder;
    Jumper jumper;

    public void Construct(MergeFieldBuilder mergeFieldBuilder, Jumper jumper)
    {
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.jumper = jumper;
    }

    public void SetParameters(PlaceTypes placeType)
    {
        this.placeType = placeType;
        withJumpAnimation = false;
    }
    public void SetParameters(PlaceTypes placeType, bool withJumpAnimation, Transform objectToJumpFrom)
    {
        this.placeType = placeType;
        this.withJumpAnimation = withJumpAnimation;
        this.objectToJumpFrom = objectToJumpFrom;
    }

    public void Place(MiningDevice miningDevice)
    {
        var availableCell = FindAvailableCell();

        if (withJumpAnimation)
        {
            PlaceToWithAnimation(miningDevice, availableCell);
        }
        else
        {
            PlaceTo(miningDevice, availableCell);
        }
    }
    public void PlaceTo(MiningDevice miningDevice, int cellNum)
    {
        PlaceTo(miningDevice, mergeFieldBuilder.Cells[cellNum]);
    }
    public void PlaceTo(MiningDevice miningDevice, Cell cell)
    {
        cell.TryInsertDevice(miningDevice);
    }
    private void PlaceToWithAnimation(MiningDevice miningDevice, Cell cell)
    {
        cell.IsBusyByAnimation = true;

        var objectToJumpFrom = this.objectToJumpFrom;

        miningDevice.transform.SetParent(objectToJumpFrom.transform);
        miningDevice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        miningDevice.transform.SetParent(jumper.transform);

        miningDevice.GetComponent<MiningDeviceBox>().StartJumpAnimation(() =>
        jumper.Jump(miningDevice.transform, cell.transform, 200, 0.2f, () =>
        {
            PlaceTo(miningDevice, cell);
            miningDevice.GetComponent<MiningDeviceBox>().Clickable = true;
            cell.IsBusyByAnimation = false;
            miningDevice.GetComponent<MiningDeviceBox>().LaunchPlaceParticles();
            transform.localScale = new Vector3(1, 1, 1);
        }, objectToJumpFrom)
        );
    }

    private Cell FindAvailableCell()
    {
        switch (placeType)
        {
            case PlaceTypes.ToRandomPossible:
                return FindRandomEmptyCell();
            default:
                throw new NotImplementedException();
        }
    }
    private Cell FindRandomEmptyCell()
    {
        var possibleCells = new List<Cell>();

        for (int i = 0; i < mergeFieldBuilder.Cells.Count; i++)
        {
            if (CanPlaceToCell(mergeFieldBuilder.Cells[i]))
            {
                possibleCells.Add(mergeFieldBuilder.Cells[i]);
            }
        }

        if (possibleCells.Count != 0)
        {
            return possibleCells[UnityEngine.Random.Range(0, possibleCells.Count)];
        }
        else
        {
            return null;
        }
    }
    private bool CanPlaceToCell(Cell cell)
    {
        return cell.IsEmpty && !cell.IsBusyByAnimation;
    }
}
