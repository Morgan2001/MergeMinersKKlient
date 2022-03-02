using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System.Linq;

public class DiamondPurchaseAnimation : MonoBehaviour
{
    public GameObject DiamondsUI;
    public Vector2 StartPos;
    public Text DiamondNum;

    private RelativePositionsCalculator relPosCalc;
    private PurchaseHandler purchaseHandler;

    public void Construct(RelativePositionsCalculator relativePositionsCalculator, PurchaseHandler purchaseHandler)
    {
        relPosCalc = relativePositionsCalculator;
        this.purchaseHandler = purchaseHandler;
    }

    public void StartAnimation(Product product)
    {
        var diamondNum = new String(product.definition.id.Where(Char.IsDigit).ToArray());
        DiamondNum.text = diamondNum;

        var endPos = relPosCalc.CalcRelativePosition(DiamondsUI, transform.parent.gameObject);

        var rt = GetComponent<RectTransform>();

        rt.anchoredPosition = StartPos;

        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1, 0.5f).From(0).SetEase(Ease.OutSine));

        seq.Append(rt.DOAnchorPos(endPos, 1).SetEase(Ease.InQuint));
        seq.Join(transform.DOScale(Vector3.zero, 1).SetEase(Ease.InQuint).OnComplete(() => purchaseHandler.HandlePurchase(product)));
    }
}
