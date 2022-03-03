using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum FlyingBonuses
{
    Wallet,
    FlashCard,
    PowerTransformer,
    Chip,
    BoxWithMiners
}

public class FlyingBonus : MonoBehaviour, IPointerDownHandler
{
    public GameObject WalletRewardPrefab;

    public FlyingBonusData Data { get; private set; }
    public int NumOfBouncesLeft { get; set; }

    private bool isOnLeftSide;
    private Vector2 posToFly;

    private FlyingBonusController flyingBonusController;
    private FlyingBonusHandler flyingBonusHandler;

    public void Construct(FlyingBonusData FlyingBonusData, bool isOnLeftSide, FlyingBonusController flyingBonusController, FlyingBonusHandler flyingBonusHandler)
    {
        Data = FlyingBonusData;
        this.flyingBonusController = flyingBonusController;
        this.isOnLeftSide = isOnLeftSide;
        this.flyingBonusHandler = flyingBonusHandler;

        NumOfBouncesLeft = FlyingBonusData.NumOfBouncesToDestroy;
        GetComponent<Image>().sprite = Data.Sprite;

        transform.DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(int.MaxValue).SetLink(gameObject);
    }

    public void FlyOrDestroy()
    {
        if (NumOfBouncesLeft > 0)
        {
            posToFly = flyingBonusController.GetNewPointToFly(!isOnLeftSide);
            LaunchFlyAnimation().OnComplete(() => 
                {
                    isOnLeftSide = !isOnLeftSide;
                    NumOfBouncesLeft--;
                    FlyOrDestroy();
                });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Tween LaunchFlyAnimation()
    {
        return GetComponent<RectTransform>().DOAnchorPos(posToFly, Data.TimeToFLyBetweenSides).SetEase(Ease.Linear).SetLink(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Data.Type == FlyingBonuses.Wallet)
        {
            CreateWalletRewardInscription();
        }

        flyingBonusHandler.HandleFlyingBonus(Data.Type);

        Destroy(gameObject);
    }

    private void CreateWalletRewardInscription()
    {
        var rewardInscription = Instantiate(WalletRewardPrefab);
        rewardInscription.transform.SetParent(transform);
        rewardInscription.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        rewardInscription.transform.SetParent(transform.parent);
        rewardInscription.transform.localScale = Vector2.one;
        rewardInscription.transform.GetChild(0).GetComponent<Text>().text = 
            flyingBonusHandler.gameRules.WalletRewards.GetRewardByLevel(flyingBonusHandler.player.MaxAchivedMiningDeviceLevel).ToString();
        Destroy(rewardInscription, 1);
    }
}
