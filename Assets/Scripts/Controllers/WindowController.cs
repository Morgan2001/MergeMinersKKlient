using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public event Action GameplayScreenIsShown;
    public event Action GameplayScreenIsHidden;

    [Header("Main")]
    public GameObject TopPanel;
    public GameObject GameplayPanel;
    public GameObject BottomPanel;

    [Header("Bottom panel tabs")]
    public GameObject UpgradePanel;
    public GameObject ShopPanel;

    [Header("FlyingBonusWindows")]
    public GameObject ChipBonusWindow;
    public GameObject PowerTransformerBonusWindow;
    public GameObject FlashCardBonusWindow;
    public GameObject BoxWithMinersBonusWindow;

    [Header("Roulette")]
    public RouletteWinWindow RouletteWinWindow;
    public GameObject Roulette;

    [Header("Different")]
    public NewMiningDeviceWindow NewMiningDeviceWindow;
    public GameObject RelocationPanel;
    public GameObject GiftWindow;
    public GameObject ReturnWindow;

    [Header("Wheel")]
    public GameObject WheelPanel;
    public GameObject WheelRewardWindow;
    

    public void ShowRelocationScreen()
    {
        HideAll();
        TopPanel.SetActive(true);
        RelocationPanel.SetActive(true);
    }
    public void ShowGameplayScreen()
    {
        HideAll();

        TopPanel.SetActive(true);
        GameplayPanel.SetActive(true);
        BottomPanel.SetActive(true);
        GameplayScreenIsShown?.Invoke();
    }
    public void ShowRouletteWinScreen()
    {
        HideAll();
        RouletteWinWindow.gameObject.SetActive(true);
    }
    public void ShowRoulette()
    {
        HideAll();
        Roulette.SetActive(true);
    }
    public void ShowNewMinerWindow()
    {
        HideAll();
        NewMiningDeviceWindow.gameObject.SetActive(true);
    }
    public void ShowChipBonusWindow()
    {
        HideAll();
        ChipBonusWindow.SetActive(true);
    }
    public void ShowPowerTransformerBonusWindow()
    {
        HideAll();
        PowerTransformerBonusWindow.SetActive(true);
    }
    public void ShowFlashCardBonusWindow()
    {
        HideAll();
        FlashCardBonusWindow.SetActive(true);
    }
    public void ShowBoxWithMinersBonusWindow()
    {
        HideAll();
        BoxWithMinersBonusWindow.SetActive(true);
    }
    public void ShowUpgradeShopWindow()
    {
        HideAll();
        UpgradePanel.SetActive(true);
        TopPanel.SetActive(true);
        BottomPanel.SetActive(true);
    }
    public void ShowGiftWindow()
    {
        HideAll();
        GiftWindow.SetActive(true);
    }
    public void ShowShopWindow()
    {
        HideAll();
        ShopPanel.SetActive(true);
        TopPanel.SetActive(true);
        BottomPanel.SetActive(true);
    }
    public void ShowWheelWindow()
    {
        HideAll();
        WheelPanel.SetActive(true);
    }
    public void ShowWheelRewardWindow(Sprite sprite, string description)
    {
        WheelRewardWindow.SetActive(true);
        WheelRewardWindow.GetComponent<WheelRewardPanel>().SetData(sprite, description);
    }
    public void ShowReturnWindow(MiningController miningController)
    {
        HideAll();
        ReturnWindow.SetActive(true);
        ReturnWindow.GetComponent<ReturnWindow>().SetData(miningController);
    }

    private void HideAll()
    {
        TopPanel.SetActive(false);
        BottomPanel.SetActive(false);
        GameplayPanel.SetActive(false);

        UpgradePanel.SetActive(false);
        ShopPanel.SetActive(false);

        ChipBonusWindow.SetActive(false);
        PowerTransformerBonusWindow.SetActive(false);
        FlashCardBonusWindow.SetActive(false);
        BoxWithMinersBonusWindow.SetActive(false);

        NewMiningDeviceWindow.gameObject.SetActive(false);
        RelocationPanel.SetActive(false);
        RouletteWinWindow.gameObject.SetActive(false);
        Roulette.SetActive(false);
        GiftWindow.SetActive(false);
        WheelPanel.SetActive(false);
        WheelRewardWindow.SetActive(false);
        ReturnWindow.SetActive(false);

        GameplayScreenIsHidden?.Invoke();
    }
}
