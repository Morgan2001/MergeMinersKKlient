using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using MergeMiner.Core.State.Enums;

[Serializable]
public class MiningDeviceBoxSprite
{
    public MinerSource Type;
    public Sprite Sprite;
}

public class MiningDeviceBox : MonoBehaviour, IPointerDownHandler
{
    public List<MiningDeviceBoxSprite> MiningDeviceBoxSprites;
    public ParticleSystem PlaceParticles;

    public float timeForAutoOpeningInSeconds;
    public bool IsInBox { get; private set; } = false;
    public bool Clickable { get; set; } = false;

    private MinerSource type;

    private GameplayHelper gameplayHelper;
    private CellHighlighter cellHighlighter;
    private WindowController windowController;
    private Roulette roulette;

    public void Construct(GameplayHelper gameplayHelper, CellHighlighter cellHighlighter, WindowController windowController, Roulette roulette)
    {
        this.gameplayHelper = gameplayHelper;
        this.cellHighlighter = cellHighlighter;
        this.windowController = windowController;
        this.roulette = roulette;
    }

    public void PutInBox(MinerSource boxType)
    {
        type = boxType;
        var miningDevice = GetComponent<MiningDevice>();

        miningDevice.Image.sprite = MiningDeviceBoxSprites.Find(mdbs => mdbs.Type == boxType).Sprite;
        miningDevice.Label.SetActive(false);
        IsInBox = true;

        if (boxType == MinerSource.Common)
        {
            StartCoroutine(OpenAfterSeconds());
        }
    }

    public IEnumerator OpenAfterSeconds()
    {
        yield return new WaitForSeconds(timeForAutoOpeningInSeconds);
        if (IsInBox)
        {
            GetOutOfBox();
            if (gameplayHelper.OpenBoxAnimationIsOn)
            {
                gameplayHelper.ResetHelp();
            }
        }
    }

    public void GetOutOfBox()
    {
        var miningDevice = GetComponent<MiningDevice>();

        miningDevice.Image.sprite = miningDevice.Data.Sprite;
        miningDevice.Label.SetActive(true);
        IsInBox = false;

        transform.parent.GetComponent<Cell>().UpdateCell();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Clickable && IsInBox)
        {
            GetOutOfBox();
            gameplayHelper.ResetHelp();
            cellHighlighter.RemoveHighlight();

            if (type == MinerSource.Random)
            {
                windowController.ShowRoulette();
                roulette.Spin(GetComponent<MiningDevice>().Data.Type, () => 
                {
                    windowController.ShowRouletteWinScreen();
                    windowController.RouletteWinWindow.SetMiningDeviceImage(GetComponent<MiningDevice>().Data);
                });
            }
        }
    }

    public void LaunchPlaceParticles()
    {
        PlaceParticles.Play();
    }

    public void StartJumpAnimation(Action callback)
    {
        GetComponent<MiningDevice>().IsClickable = false;

        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(new Vector3(1.4f, 1f), 0.1f));
        seq.Join(GetComponent<RectTransform>().
            DOAnchorPos(GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 50), 0.2f));
        seq.Append(transform.DOScale(new Vector3(1f, 1.3f), 0.1f));

        seq.AppendCallback(() => 
        {
            GetComponent<MiningDevice>().IsClickable = true;
            callback.Invoke();
        });
    }
}
