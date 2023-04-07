using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private Transform bufferForDraggedItem;
    private EventSystem eventSystem;
    private Jumper jumper;
    private CellHighlighter cellHighlighter;
    private GameplayHelper gameplayHelper;
    private TrashCan trashCan;

    private MiningDevice miningDevice;

    private Cell prevCell;
    private Cell cellToInsertDevice;
    private Tween followTween;
    private bool drugStarted = false;


    public void Construct(Canvas canvas, Transform bufferForDraggedItem, EventSystem eventSystem, 
        Jumper jumper, CellHighlighter cellHighlighter, GameplayHelper gameplayHelper, TrashCan trashCan)
    {
        this.canvas = canvas;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        this.bufferForDraggedItem = bufferForDraggedItem;
        this.eventSystem = eventSystem;
        this.jumper = jumper;
        this.cellHighlighter = cellHighlighter;
        this.gameplayHelper = gameplayHelper;
        this.trashCan = trashCan;

        miningDevice = GetComponent<MiningDevice>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (miningDevice.IsClickable)
        {
            drugStarted = true;

            prevCell = transform.parent.GetComponent<Cell>();
            PlaceDeviceInBuffer();
            transform.DOScale(1.3f, 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            cellHighlighter.HighlightAllPossibleMerges(prevCell);
            gameplayHelper.ResetHelp();
            trashCan.SetInactiveSprite();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (miningDevice.IsClickable && drugStarted)
        {
            FollowPointer();
            gameplayHelper.ResetHelp();
            var trashCan = FindTrashCan(CastRayOnPointer());

            if (trashCan != null)
            {
                this.trashCan.SetActiveSprite();
            }
            else
            {
                this.trashCan.SetInactiveSprite();
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (miningDevice.IsClickable && drugStarted)
        {
            drugStarted = false;

            StopFollowingPointer();

            cellToInsertDevice = FindCell(CastRayOnPointer());

            var cellIsFound = cellToInsertDevice != null;

            if (cellIsFound && cellToInsertDevice.CanInsertDevice(GetComponent<MiningDevice>()))
            {
                JumpToNewCell();
            }
            else if (cellIsFound)
            {
                JumpSwap();
            }
            else
            {
                JumpBack();
            }

            transform.DOScale(1, 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            cellHighlighter.RemoveHighlight();
            this.trashCan.SetOriginalSprite();

            var trashCan = FindTrashCan(CastRayOnPointer());

            if (trashCan != null)
            {
                prevCell.RemoveDevice();
                Destroy(gameObject);
            }
        }
    }
    private void JumpToNewCell()
    {
        jumper.Jump(transform.GetComponent<RectTransform>(), cellToInsertDevice.GetComponent<RectTransform>(), 40, 0.1f, () =>
        {
            prevCell.RemoveDevice();
            cellToInsertDevice.TryInsertDevice(GetComponent<MiningDevice>());
        });
    }
    private void JumpSwap()
    {
        var DeviceInNewCell = cellToInsertDevice.MiningDevice;

        jumper.Jump(transform.GetComponent<RectTransform>(), cellToInsertDevice.GetComponent<RectTransform>(), 40, 0.1f, () =>
        {

        });
        jumper.Jump(DeviceInNewCell.GetComponent<RectTransform>(), prevCell.GetComponent<RectTransform>(), 40, 0.1f, () =>
        {
            cellToInsertDevice.Swap(prevCell);
        });
    }
    private void JumpBack()
    {
        jumper.Jump(transform.GetComponent<RectTransform>(), prevCell.GetComponent<RectTransform>(), 40, 0.1f, () =>
        {
            PlaceDeviceBackFromBuffer();
        });
    }

    private void PlaceDeviceInBuffer()
    {
        transform.SetParent(bufferForDraggedItem);
    }
    private void PlaceDeviceBackFromBuffer()
    {
        transform.SetParent(prevCell.transform);
    }
    private void FollowPointer()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        StopFollowingPointer();
        followTween = transform.DOMove(canvas.transform.TransformPoint(pos), 0.15f).SetLink(gameObject);
    }
    private void StopFollowingPointer()
    {
        followTween?.Kill();
    }

    private Cell FindCell(List<RaycastResult> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            var curCell = results[i].gameObject.GetComponent<Cell>();

            if (curCell != null)
            {
                return curCell;
            }
        }

        return null;
    }
    private TrashCan FindTrashCan(List<RaycastResult> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            var trashCan = results[i].gameObject.GetComponent<TrashCan>();

            if (trashCan != null)
            {
                return trashCan;
            }
        }

        return null;
    }
    private List<RaycastResult> CastRayOnPointer()
    {
        var pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        return results;
    }
}
