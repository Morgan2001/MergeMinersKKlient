using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHelper : MonoBehaviour
{
    public float IdleTimeForStartHelp;
    private float timeLeft;

    public GameObject Pointer;
    public GameObject PointerPanel;
    private OperationsOnField operationsOnField;
    private RelativePositionsCalculator relativePosCalc;
    private Box box;

    private bool animationIsOn = false;
    public bool OpenBoxAnimationIsOn { get; private set; } = false;
    public bool TapBoxAnimationIsOn { get; private set; } = false;

    private Sequence AnimationSeq;

    private (Cell first, Cell second) cellsForMergeTip;
    private Cell emptyMiningSlot;
    private Cell cellWithDeviceNotInMiningSlot;
    private Cell cellWithUnpackedDevice;

    public float TimeLeft
    {
        get => timeLeft;
        set
        {
            timeLeft = value;

            if (timeLeft <= 0)
            {
                Help();
            }
        }
    }

    public void Construct(OperationsOnField operationsOnField, RelativePositionsCalculator relativePosCalc, Box box)
    {
        this.operationsOnField = operationsOnField;
        this.relativePosCalc = relativePosCalc;
        this.box = box;
        ResetTimeLeft();
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
    }

    public void ResetTimeLeft()
    {
        timeLeft = IdleTimeForStartHelp;
    }

    public void Help()
    {
        if (!animationIsOn)
        {
            cellsForMergeTip = operationsOnField.FindCellsWithIdenticalMiningDevices();

            if (cellsForMergeTip != (null, null))
            {
                LaunchMergeTip();
                return;
            }

            emptyMiningSlot = operationsOnField.FindEmptyMiningSlot();
            cellWithDeviceNotInMiningSlot = operationsOnField.FindNotMiningCellWithMiningDeviceNotInBox();

            if (emptyMiningSlot != null && cellWithDeviceNotInMiningSlot != null)
            {
                LaunchEmptySlotTip();
                return;
            }

            cellWithUnpackedDevice = operationsOnField.FindCellWithUnopenedBox();

            if (cellWithUnpackedDevice != null)
            {
                LaunchOpenBoxTip();
                return;
            }

            StartAnimation(box.gameObject);
            TapBoxAnimationIsOn = true;
        }
    }

    private void LaunchOpenBoxTip()
    {
        StartAnimation(cellWithUnpackedDevice, cellWithUnpackedDevice);

        OpenBoxAnimationIsOn = true;
    }

    private void LaunchEmptySlotTip()
    {
        StartAnimation(cellWithDeviceNotInMiningSlot, emptyMiningSlot);

        cellWithDeviceNotInMiningSlot.Highlight();
    }

    private void LaunchMergeTip()
    {
        StartAnimation(cellsForMergeTip.first, cellsForMergeTip.second);

        cellsForMergeTip.first.Highlight();
        cellsForMergeTip.second.Highlight();
    }

    private void StartAnimation(Cell from, Cell to)
    {
        var fromPos = relativePosCalc.CalcRelativePositions(
            from.GetComponent<RectTransform>(),
            PointerPanel.GetComponent<RectTransform>());
        var toPos = relativePosCalc.CalcRelativePositions(
            to.GetComponent<RectTransform>(),
            PointerPanel.GetComponent<RectTransform>());
        StartDragAnimation(fromPos, toPos);
    }
    private void StartAnimation(GameObject go)
    {
        var pos = relativePosCalc.CalcRelativePositions(
            go.GetComponent<RectTransform>(),
            PointerPanel.GetComponent<RectTransform>());
        StartDragAnimation(pos, pos);
    }

    private void StartDragAnimation(Vector2 fromPos, Vector2 toPos)
    {
        AnimationSeq = DOTween.Sequence();
        AnimationSeq.Append(Pointer.GetComponent<Image>().DOFade(1, 0));
        AnimationSeq.Append(Pointer.GetComponent<RectTransform>().DOAnchorPos(toPos, 0.9f).From(fromPos).SetEase(Ease.Linear));
        AnimationSeq.Join(Pointer.transform.DOScale(Vector3.one, 0.3f).From(Vector3.one * 1.8f).SetEase(Ease.Linear));
        AnimationSeq.Append(Pointer.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear));
        AnimationSeq.SetLoops(int.MaxValue);

        animationIsOn = true;
    }

    public void KillAnimation()
    {
        AnimationSeq.Kill();
        animationIsOn = false;
        Pointer.GetComponent<Image>().DOFade(0, 0);
        OpenBoxAnimationIsOn = false;
        TapBoxAnimationIsOn = false;
    }

    public void ResetHelp()
    {
        ResetTimeLeft();
        KillAnimation();
    }
}
