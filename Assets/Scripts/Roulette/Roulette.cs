using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    public Vector2 FromPos;
    [Header("Cells")]
    public GameObject WinCell;
    public List<GameObject> OtherCells;

    private P.Player player;
    private GameRules gameRules;

    private MiningDevices desiredMiningDevice;

    public void Construct(P.Player player, GameRules gameRules)
    {
        this.player = player;
        this.gameRules = gameRules;
    }

    public void Spin(MiningDevices desiredMiningDevice, Action callback)
    {
        this.desiredMiningDevice = desiredMiningDevice;
        FillWinCell();
        FillNotWinCells();

        var seq = DOTween.Sequence();
        seq.Append(GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1).SetEase(Ease.OutQuad).From(FromPos));
        seq.AppendInterval(1);
        seq.OnComplete(() => { callback.Invoke(); });
    }

    private void FillWinCell()
    {
        SetDeviceToCell(WinCell, gameRules.MiningDevices[desiredMiningDevice]);
    }
    private void FillNotWinCells()
    {
        var fromDeviceLevel = 1;
        var toDeviceLevel = player.MaxAchivedMiningDeviceLevel;

        foreach (var cell in OtherCells)
        {
            var randomLevel = UnityEngine.Random.Range(fromDeviceLevel, toDeviceLevel + 1);
            var data = gameRules.MiningDevices[randomLevel];

            SetDeviceToCell(cell, data);
        }
    }
    private void SetDeviceToCell(GameObject cell, MiningDeviceData data)
    {

        cell.transform.GetChild(0).GetComponent<Image>().sprite = data.Sprite;
        cell.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = data.Level.ToString();
    }
}
