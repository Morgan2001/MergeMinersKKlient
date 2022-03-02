using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [Header("Appearance")]
    public Sprite EmptyCellSprite;
    public Sprite CellWithDeviceSprite;
    public GameObject HighlightPrefab;
    public GameObject MiningBackPrefab;

    private MiningDevice miningDevice;
    private MiningDeviceMerger miningDeviceMerger;

    public bool IsBusyByAnimation { get; set; }
    public bool IsMiningCell { get; private set; }
    public bool IsEmpty { get => miningDevice == null; }
    public MiningDevice MiningDevice { get => miningDevice; }

    public void Construct(MiningDeviceMerger miningDeviceMerger, bool IsMiningCell)
    {
        this.miningDeviceMerger = miningDeviceMerger;
        this.IsMiningCell = IsMiningCell;
        if (IsMiningCell)
        {
            SetMiningPrefab();
        }
    }

    public void TryInsertDevice(MiningDevice miningDevice)
    {
        if (IsEmpty)
        {
            InsertDevice(miningDevice);
        }
        else
        {
            miningDeviceMerger.TryMerge(this.miningDevice, miningDevice);
        }

        UpdateCell();
    }
    private void InsertDevice(MiningDevice miningDevice)
    {
        this.miningDevice = miningDevice;

        miningDevice.transform.SetParent(transform);
        miningDevice.transform.localScale = Vector3.one;
        miningDevice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        miningDevice.Image.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        miningDevice.GetComponent<MiningDeviceBox>().Clickable = true;
    }
    public void UpdateCell()
    {
        UpdateCellAppearance();
        UpdateMiningDeviceShaking();
        UpdateMiningDeviceAppearance();
    }

    private void UpdateMiningDeviceAppearance()
    {
        if (!IsEmpty && !miningDevice.GetComponent<MiningDeviceBox>().IsInBox)
        {
            if (IsMiningCell)
            {
                miningDevice.SetOutlinedSprite();
            }
            else
            {
                miningDevice.SetCommonSprite();
            }
        }
    }

    private void UpdateCellAppearance()
    {
        if (!IsEmpty)
        {
            if (miningDevice.GetComponent<MiningDeviceBox>().IsInBox)
            {
                SetEmptyBack();
            }
            else
            {
                if (IsMiningCell)
                {
                    SetEmptyBack();
                }
                else
                {
                    SetWhiteBack();
                }
            }
        }
        else
        {
            SetEmptyBack();
        }
    }
    private void UpdateMiningDeviceShaking()
    {
        if (!IsEmpty)
        {
            if (miningDevice.GetComponent<MiningDeviceBox>().IsInBox)
            {
                miningDevice.GetComponent<ShakeAnimation>().StopShaking();
            }
            else
            {
                if (IsMiningCell)
                {
                    miningDevice.GetComponent<ShakeAnimation>().StartShaking();
                }
                else
                {
                    miningDevice.GetComponent<ShakeAnimation>().StopShaking();
                }
            }
        }
    }

    public void Swap(Cell cellToSwapWith)
    {
        var cellToSwapWithDevice = cellToSwapWith.RemoveDevice();
        cellToSwapWith.TryInsertDevice(RemoveDevice());
        TryInsertDevice(cellToSwapWithDevice);
    }

    public bool CanInsertDevice(MiningDevice miningDevice)
    {
        if (IsEmpty)
        {
            return true;
        }
        else if (miningDeviceMerger.CanMerge(this.miningDevice, miningDevice))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetEmptyBack()
    {
        GetComponent<Image>().sprite = EmptyCellSprite;
    }
    private void SetMiningPrefab()
    {
        MiningBackPrefab.SetActive(true);
    }
    private void RemoveMiningPrefab()
    {
        MiningBackPrefab.SetActive(false);
    }
    private void SetWhiteBack()
    {
        GetComponent<Image>().sprite = CellWithDeviceSprite;
    }
    public void Highlight()
    {
        HighlightPrefab.SetActive(true);
    }
    public void RemoveHighlight()
    {
        HighlightPrefab.SetActive(false);
    }
    public void BecomeMiningCell()
    {
        IsMiningCell = true;
        SetMiningPrefab();
        UpdateCell();
    }
    public void BecomeNotMiningCell()
    {
        IsMiningCell = false;
        RemoveMiningPrefab();
        UpdateCell();
    }

    public MiningDevice RemoveDevice()
    {
        var miningDeviceRef = miningDevice;
        miningDevice = null;
        SetEmptyBack();

        return miningDeviceRef;
    }
}
