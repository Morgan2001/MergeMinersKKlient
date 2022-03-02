using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBinder : MonoBehaviour
{
    [Header("Upgrade shop")]
    public Button GoToUpgradeShopButton;

    [Header("Shop")]
    public Button GoToShopButton;

    [Header("Relocation")]
    public Button GoToRelocateScreenButton;
    public Button StartRelocateButton;

    [Header("Flying bonuses")]
    public Button ChipButton;
    public Button FlashCardButton;
    public Button PowerTransformerButton;
    public Button BoxWithMiners;

    [Header("Gift")]
    public Button GiftButton;

    [Header("Wheel")]
    public Button WheelForAdButton;
    public Button WheelForDiamondsButton;

    [Header("Quit")]
    public Button QuitButton;

    public List<Button> ToGameplayScreenButtons;


    private WindowController windowController;
    private Relocator relocator;
    private BoostController boostController;
    private MergeFieldFiller mergeFieldFiller;
    private Gift gift;
    private Ads ads;
    private Wheel wheel;
    private Player player;

    public void Construct(WindowController windowController, Relocator relocator, BoostController boostController, MergeFieldFiller mergeFieldFiller,
        Gift gift, Ads ads, Wheel wheel, Player player)
    {
        this.windowController = windowController;
        this.relocator = relocator;
        this.boostController = boostController;
        this.mergeFieldFiller = mergeFieldFiller;
        this.gift = gift;
        this.ads = ads;
        this.wheel = wheel;
        this.player = player;

        Bind();
    }

    private void Bind()
    {
        BindGoToRelocateScreenButton();
        BindStartRelocateButton();
        BindApplyChipFlyingBonusButton();
        BindApplyFlashCardBonusButton();
        BindApplyPowerTransformerBonusButton();
        BindApplyBoxWithMinersBonusButton();
        BindGoToUpgradeShopButton();
        BindGiftButton();
        BindGoToShopButton();
        BindWheelForAdButton();
        BindWheelForDiamondsButton();
        BindQuitButton();

        BindGoToGameplayScreenButtons();
    }

    private void BindQuitButton()
    {
        QuitButton.onClick.AddListener(Application.Quit);
    }

    private void BindWheelForDiamondsButton()
    {
        WheelForDiamondsButton.onClick.AddListener(() =>
        {
            if (player.Diamonds >= 5)
            {
                player.SpendDiamonds(5);
                wheel.Spin(SpinPriceTypes.ForDiamonds);
            }
        });
    }

    private void BindWheelForAdButton()
    {
        WheelForAdButton.onClick.AddListener(() =>
            {
                if (wheel.AttemptsForDayLeft > 0)
                {
                    ads.ShowRewarded(() => wheel.Spin(SpinPriceTypes.ForAd));
                }
            }
        );
    }

    private void BindGoToShopButton()
    {
        GoToShopButton.onClick.AddListener(windowController.ShowShopWindow);
        GoToShopButton.onClick.AddListener(ads.ShowInterstitial);
    }

    private void BindApplyBoxWithMinersBonusButton()
    {
        BoxWithMiners.onClick.AddListener(() =>
        {
            ads.ShowRewarded(() => mergeFieldFiller.AddDevicesFromBoxWithMiners());
        });
    }

    private void BindGoToUpgradeShopButton()
    {
        GoToUpgradeShopButton.onClick.AddListener(windowController.ShowUpgradeShopWindow);
        GoToUpgradeShopButton.onClick.AddListener(ads.ShowInterstitial);
    }

    private void BindApplyPowerTransformerBonusButton()
    {
        PowerTransformerButton.onClick.AddListener(() =>
        {
            ads.ShowRewarded(() => boostController.Boost(Boosts.EverySlotIsMining, 1, 5));
        });
    }

    private void BindApplyFlashCardBonusButton()
    {
        FlashCardButton.onClick.AddListener(() =>
        {
            ads.ShowRewarded(() => boostController.Boost(Boosts.MiningPower, 10, 5));
        });
    }

    private void BindApplyChipFlyingBonusButton()
    {
        ChipButton.onClick.AddListener(() =>
        {
            ads.ShowRewarded(() => boostController.Boost(Boosts.BoxFillingSpeed, 3, 5));
        });
    }

    private void BindGoToGameplayScreenButtons()
    {
        foreach (var button in ToGameplayScreenButtons)
        {
            button.onClick.AddListener(windowController.ShowGameplayScreen);
        }
    }

    private void BindStartRelocateButton()
    {
        StartRelocateButton.onClick.AddListener(() =>
        {
            relocator.RelocateToNext();
        });
    }

    private void BindGoToRelocateScreenButton()
    {
        GoToRelocateScreenButton.onClick.AddListener(windowController.ShowRelocationScreen);
    }

    private void BindGiftButton()
    {
        GiftButton.onClick.AddListener(gift.TryGiveOutGift);
    }
}
