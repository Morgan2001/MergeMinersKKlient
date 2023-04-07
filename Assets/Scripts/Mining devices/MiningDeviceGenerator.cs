using System.Collections;
using System.Collections.Generic;
using MergeMiner.Core.State.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiningDeviceGenerator : MonoBehaviour
{
    public GameObject MiningDevicePrefab;

    private GameRules gameRules;
    private Canvas canvas;
    private Transform bufferForDraggedItem;
    private EventSystem eventSystem;
    private Jumper jumper;
    private MergeFieldBuilder mergeFieldBuilder;
    private CellHighlighter cellHighlighter;
    private GameplayHelper gameplayHelper;
    private WindowController windowController;
    private Roulette roulette;
    private TrashCan trashCan;


    public void Construct(GameRules gameRules, Canvas canvas, 
        Transform bufferForDraggedItem, EventSystem eventSystem, Jumper jumper, 
        MergeFieldBuilder mergeFieldBuilder, CellHighlighter cellHighlighter, GameplayHelper gameplayHelper,
        WindowController windowController, Roulette roulette, TrashCan trashCan)
    {
        this.canvas = canvas;
        this.bufferForDraggedItem = bufferForDraggedItem;
        this.eventSystem = eventSystem;
        this.gameRules = gameRules;
        this.jumper = jumper;
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.cellHighlighter = cellHighlighter;
        this.gameplayHelper = gameplayHelper;
        this.windowController = windowController;
        this.roulette = roulette;
        this.trashCan = trashCan;
    }

    public MiningDevice CreateMiningDevice(MiningDevices type, bool inBox = true, MinerSource boxType = MinerSource.Common)
    {
        var miningDeviceGO = Instantiate(MiningDevicePrefab);
        miningDeviceGO.transform.SetParent(jumper.transform);
        miningDeviceGO.transform.localScale = Vector3.one;
        miningDeviceGO.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(mergeFieldBuilder.CellSize, mergeFieldBuilder.CellSize);

        var miningDevice = miningDeviceGO.GetComponent<MiningDevice>();
        miningDevice.Construct(gameRules.MiningDevices[type]);

        var dragAndDrop = miningDeviceGO.GetComponent<DragAndDrop>();
        dragAndDrop.Construct(canvas, bufferForDraggedItem, eventSystem, jumper, cellHighlighter, gameplayHelper, trashCan);

        if (inBox)
        {
            miningDeviceGO.GetComponent<MiningDeviceBox>().Construct(gameplayHelper, cellHighlighter, windowController, roulette);
            miningDeviceGO.GetComponent<MiningDeviceBox>().PutInBox(boxType);
        }

        return miningDevice;
    }
}
