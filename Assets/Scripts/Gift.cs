using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public float TimeToGift;

    public bool IsReady { get; private set; }
    private float curTimeToGift;
    public float CurTimeToGift {
        get => curTimeToGift; 
        private set
        {
            curTimeToGift = value;

            if (CurTimeToGift / TimeToGift >= 1)
            {
                IsReady = true;
                giftIndicator.UpdateFillAmount(1);
                buttonHighlight.TurnOn();
            }
            else
            {
                IsReady = false;
                giftIndicator.UpdateFillAmount(CurTimeToGift / TimeToGift);
                buttonHighlight.TurnOff();
            }
        } 
    }

    private Player player;
    private WindowController windowController;
    private FillIndicatorUI giftIndicator;
    private ButtonHighlight buttonHighlight;

    public void Construct(Player player, WindowController windowController, FillIndicatorUI giftIndicator, ButtonHighlight buttonHighlight)
    {
        this.player = player;
        this.windowController = windowController;
        this.giftIndicator = giftIndicator;
        this.buttonHighlight = buttonHighlight;
        ResetProgress();
    }

    private void Update()
    {
        CurTimeToGift += Time.deltaTime;
    }

    public void TryGiveOutGift()
    {
        if (IsReady)
        {
            player.AddDiamonds(1);
            ResetProgress();
            windowController.ShowGiftWindow();
        }
    }

    public void ResetProgress()
    {
        CurTimeToGift = 0;
    }
}
