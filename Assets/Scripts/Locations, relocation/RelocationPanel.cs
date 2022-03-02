using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelocationPanel : MonoBehaviour
{
    public Text Name;
    public Text Level;

    public Text Slots;
    public Text Sockets;
    public Text MaxMiningDeviceLevel;

    public Text ButtonText;
    public Button RelocateButton;
    public Color TextColorWhenActive;
    public Color TextColorWhenInactive;

    public Image LocationImage;

    LocationData curLoc;
    LocationData nextLoc;

    Player player;
    GameRules gameRules;

    public void Construct(Player player, GameRules gameRules)
    {
        this.player = player;
        this.gameRules = gameRules;
    }

    private void OnEnable()
    {
        ShowInfoAboutNextLocation();

        if (gameRules.MiningDevices[player.MaxAchivedMiningDeviceLevel].Level < nextLoc.MinMiningDeviceLevelToRelocate)
        {
            SetButtonInactive();
            ButtonText.text = $"Assemble level {nextLoc.MinMiningDeviceLevelToRelocate} miner first";
        }
        else if (player.Coins < nextLoc.RelocateCost)
        {
            SetButtonInactive();
            ButtonText.text = $"Relocate\n" + $"{nextLoc.RelocateCost} coins";
        }
        else
        {
            SetButtonActive();
            ButtonText.text = $"Relocate\n" + $"{nextLoc.RelocateCost} coins";
        }
    }

    private void ShowInfoAboutNextLocation()
    {
        curLoc = gameRules.Locations[player.CurLocation];
        nextLoc = gameRules.Locations.LocationDatas.Find(location => location.Level == curLoc.Level + 1);

        Name.text = nextLoc.Name;
        Level.text = $"Level {nextLoc.Level}";

        Slots.text = (nextLoc.NumOfColumns * nextLoc.NumOfRows).ToString();
        Sockets.text = nextLoc.NumOfMiningCells.ToString();
        MaxMiningDeviceLevel.text = nextLoc.MaxLevelOfMiningDevice.ToString();

        LocationImage.sprite = nextLoc.Sprite;
    }

    private void SetButtonInactive()
    {
        RelocateButton.interactable = false;
        ButtonText.color = TextColorWhenInactive;
    }

    private void SetButtonActive()
    {
        RelocateButton.interactable = true;
        ButtonText.color = TextColorWhenActive;
    }
}
