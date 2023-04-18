using System;
using System.Collections;
using System.Collections.Generic;
using P;
using UnityEngine;

public class UpgradeShopController : MonoBehaviour
{
    public GameObject UpgradeShopPositionForDiamondsPrefab;
    public GameObject UpgradeShopPositionForAdsPrefab;

    public List<UpgradeOffer> upgradeOffers;
    private List<UpgradeShopPosition> upgradeShopPositions;

    private BoostController boostController;
    private P.Player player;
    private Ads ads;
    
    public void Construct(BoostController boostController, P.Player player, Ads ads)
    {
        upgradeShopPositions = new List<UpgradeShopPosition>();

        this.boostController = boostController;
        this.player = player;
        this.ads = ads;

        AddPositions();
    }

    private void AddPositions()
    {
        foreach (var upgradeOffer in upgradeOffers)
        {
            AddPosition(upgradeOffer);
        }
    }

    private void AddPosition(UpgradeOffer upgradeOffer)
    {
        GameObject posGO;
        if (upgradeOffer.Currency == ResourceTypes.Diamonds)
        {
            posGO = Instantiate(UpgradeShopPositionForDiamondsPrefab, transform);
        }
        else
        {
            posGO = Instantiate(UpgradeShopPositionForAdsPrefab, transform);
        }
        var pos = posGO.GetComponent<UpgradeShopPosition>();
        pos.Construct(boostController, player, upgradeOffer, ads);
        upgradeShopPositions.Add(pos);
    }
}
