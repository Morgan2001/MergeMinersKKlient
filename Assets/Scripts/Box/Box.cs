using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Box : MonoBehaviour, IPointerDownHandler
{
    public float fillPercentPerSecond;
    public float fillPercentPerClick;

    private float curFill = 0;

    private const float chanceOfRandomDevice = 0.15f;

    private bool isOnPause;

    private BoxUI boxUI;
    private MergeFieldFiller mergeFieldFiller;
    private MergeFieldBuilder mergeFieldBuilder;
    private GameplayHelper gameplayHelper;
    private WindowController windowController;
    private MiningDeviceChooser miningDeviceChooser;

    public float Boost { get; set; } = 1;

    public float CurFill
    {
        get => curFill;
        private set
        {
            if (value >= 1)
            {
                if (mergeFieldBuilder.HasEmptyCells())
                {
                    curFill = 0;
                    AddBoxWithMiningDevice();

                    if (gameplayHelper.TapBoxAnimationIsOn)
                    {
                        gameplayHelper.ResetHelp();
                    }
                }
                else
                {
                    curFill = 1;
                }
            }
            else
            {
                curFill = value;
            }

            boxUI.UpdateFillAmount(curFill);
        }
    }
    public void Construct(MergeFieldFiller mergeFieldFiller, MergeFieldBuilder mergeFieldBuilder, 
        GameplayHelper gameplayHelper, WindowController windowController, MiningDeviceChooser miningDeviceChooser)
    {
        boxUI = GetComponent<BoxUI>();
        this.mergeFieldFiller = mergeFieldFiller;
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.gameplayHelper = gameplayHelper;
        this.windowController = windowController;
        this.miningDeviceChooser = miningDeviceChooser;

        windowController.GameplayScreenIsHidden += Pause;
        windowController.GameplayScreenIsShown += Resume;

        Resume();
    }
    public void Update()
    {
        if (!isOnPause)
        {
            AddPercentToCurFill(fillPercentPerSecond * Time.deltaTime);
        }
    }

    private void AddBoxWithMiningDevice()
    {
        var boxType = ChooseBoxType();
        var miningDeviceType = miningDeviceChooser.GetMiningDeviceFromBox().Type;

        if (boxType == MiningDeviceBoxes.Common)
        {
            mergeFieldFiller.AddDeviceFromBox(miningDeviceType, boxType, transform);
        }
        else
        {
            var randomMiningDeviceType = miningDeviceChooser.ChooseRandomMiningDeviceFromBoxToMaxAchived();

            mergeFieldFiller.AddDeviceFromBox(randomMiningDeviceType, boxType, transform);
        }
    }
    private MiningDeviceBoxes ChooseBoxType()
    {
        var rn = UnityEngine.Random.Range(0f, 1f);

        if (rn < chanceOfRandomDevice)
        {
            return MiningDeviceBoxes.Random;
        }
        else
        {
            return MiningDeviceBoxes.Common;
        }
    }
    public void AddPercentToCurFill(float percent)
    {
        CurFill += percent * Boost;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isOnPause)
        {
            AddPercentToCurFill(fillPercentPerClick);
            gameplayHelper.ResetHelp();
        }
        else
        {
            windowController.ShowGameplayScreen();
        }
    }

    private void Pause()
    {
        isOnPause = true;
    }
    private void Resume()
    {
        isOnPause = false;
    }
}
