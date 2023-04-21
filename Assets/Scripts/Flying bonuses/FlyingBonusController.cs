using System.Collections;
using UnityEngine;
using System;
using MergeMiner.Core.State.Config;

public class FlyingBonusController : MonoBehaviour
{
    public GameObject FlyingBonusPrefab;

    public FlyingBonusPanelMarker LeftMarker;
    public FlyingBonusPanelMarker RightMarker;

    private const int DelayBetweenCreatingNewBonusInSeconds = 20;

    private RelativePositionsCalculator relativePositionsCalculator;
    private GameRules gameRules;
    private FlyingBonusHandler flyingBonusHandler;

    public void Construct(RelativePositionsCalculator relativePositionsCalculator, GameRules gameRules, FlyingBonusHandler flyingBonusHandler)
    {
        this.relativePositionsCalculator = relativePositionsCalculator;
        this.gameRules = gameRules;
        this.flyingBonusHandler = flyingBonusHandler;
    }

    private void OnEnable()
    {
        StartCoroutine(CreateAndLaunchNewBonusAfterTime());
    }

    public void CreateAndLaunchBonus()
    {
        // var flyingBonusType = ChooseBonusType();
        // var isOnLeftSide = ChooseSide();
        // var flyingBonus = GenerateFlyingBonus(flyingBonusType, isOnLeftSide, this);
        // flyingBonus.FlyOrDestroy();
    }

    // private BonusType ChooseBonusType()
    // {
        // var enumValues = Enum.GetValues(typeof(BonusType));

        // return (BonusType)UnityEngine.Random.Range(0, enumValues.Length);
    // }

    private bool ChooseSide()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    // private FlyingBonus GenerateFlyingBonus(BonusType type, bool isOnLeftSide, FlyingBonusController flyingBonusController)
    // {
        // var startPos = GetNewPointToFly(isOnLeftSide);
        // var flyingBonusGO = Instantiate(FlyingBonusPrefab);
        // flyingBonusGO.transform.SetParent(transform);
        // flyingBonusGO.GetComponent<RectTransform>().anchoredPosition = startPos;
        // flyingBonusGO.transform.localScale = Vector3.one;
        // var flyingBonus = flyingBonusGO.GetComponent<FlyingBonus>();
        // flyingBonus.Construct(gameRules.SetOfFlyingBonuses[type], isOnLeftSide, flyingBonusController, flyingBonusHandler);

        // return flyingBonus;
    // }

    public Vector2 GetNewPointToFly(bool onLeftSide)
    {
        FlyingBonusPanelMarker marker;

        if (onLeftSide)
        {
            marker = LeftMarker.SetToRandomPoint();
        }
        else
        {
            marker = RightMarker.SetToRandomPoint();
        }
        return relativePositionsCalculator.CalcRelativePosition(marker.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>());
    }

    private IEnumerator CreateAndLaunchNewBonusAfterTime()
    {
        yield return new WaitForSeconds(DelayBetweenCreatingNewBonusInSeconds);
        CreateAndLaunchBonus();
        StartCoroutine(CreateAndLaunchNewBonusAfterTime());
    }
}
