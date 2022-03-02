using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningDeviceShop : MonoBehaviour, ISavable
{
    public GameObject MiningDevicePositionPrefab;
    public GameObject MiningDeviceAdPositionPrefab;
    public ScrollRect ScrollRect;

    public List<MiningDeviceOffer> AllExistingOffers;
    public List<MiningDeviceOffer> AllExistingAdOffers;

    private List<MiningDeviceOffer> availableOffers;
    private List<MiningDevicePosition> positions;
    public List<OfferBoughtNum> OffersBoughtNum { get; private set; }

    private Player player;
    private GameRules gameRules;
    private MergeFieldFiller mergeFieldFiller;
    private MergeFieldBuilder mergeFieldBuilder;
    private Ads ads;
    private Subscription subscription;

    public void Construct(Player player, GameRules gameRules, MergeFieldFiller mergeFieldFiller, MergeFieldBuilder mergeFieldBuilder, Ads ads, Subscription subscription)
    {
        this.player = player;
        player.MaxAchivedMiningDeviceLevelUpdated += ShowAvailablePositions;
        this.gameRules = gameRules;
        this.mergeFieldFiller = mergeFieldFiller;
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.ads = ads;
        this.subscription = subscription;
        
        Load();

        availableOffers = new List<MiningDeviceOffer>();
        positions = new List<MiningDevicePosition>();

        ShowAvailablePositions();
    }

    

    public void ShowAvailablePositions()
    {
        var srPos = ScrollRect.horizontalNormalizedPosition;

        UpdateAvailableOffers();
        ClearPositions();
        AddPositionsAccordingToOffers();

        ScrollRect.horizontalNormalizedPosition = srPos;
    }

    private void UpdatePositionsAccordingToOffers()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            var miningDevicePrice = CalcPrice(availableOffers[i]);
            var miningDeviceData = gameRules.MiningDevices[availableOffers[i].MiningDeviceLevel];

            positions[i].SetOffer(miningDevicePrice, miningDeviceData);
        }
    }

    private void AddPositionsAccordingToOffers()
    {
        foreach (var offer in availableOffers)
        {
            MiningDevicePosition position = null;
            if (offer.Currency == ResourceTypes.Coins)
            {
                position = Instantiate(MiningDevicePositionPrefab, transform).GetComponent<MiningDevicePosition>();
                position.SetOffer(CalcPrice(offer), gameRules.MiningDevices[offer.MiningDeviceLevel]);
            }
            else if (offer.Currency == ResourceTypes.Ads)
            {
                position = Instantiate(MiningDeviceAdPositionPrefab, transform).GetComponent<MiningDevicePosition>();
                position.SetOffer(1, gameRules.MiningDevices[offer.MiningDeviceLevel]);
            }

            position.GetComponent<Button>().onClick.AddListener(() =>  Buy(offer, position));

            positions.Add(position);
        }
    }

    private void Buy(MiningDeviceOffer offer, MiningDevicePosition position)
    {
        if (mergeFieldBuilder.HasEmptyCells()) 
        {
            if (offer.Currency == ResourceTypes.Coins)
            {
                var price = CalcPrice(offer);

                if (player.Coins >= price)
                {
                    mergeFieldFiller.AddDeviceFromShop(gameRules.MiningDevices[offer.MiningDeviceLevel].Type, position.transform);
                    player.SpendCoins(price);
                    OffersBoughtNum.Find(x => x.OfferId == offer.Id).IncreaseBoughtNum();

                    UpdatePositionsAccordingToOffers();
                }
            }
            else if (offer.Currency == ResourceTypes.Ads)
            {
                ads.ShowRewarded(() => 
                {
                    mergeFieldFiller.AddDeviceFromShop(gameRules.MiningDevices[offer.MiningDeviceLevel].Type, position.transform);
                });
            }
        }
    }

    private double CalcPrice(MiningDeviceOffer offer)
    {
        var mult = 1f;

        if (subscription.IsActive())
        {
            mult -= subscription.Data.SaleInMiningDeviceShop;
        }

        var howMuchTimesWasBought = OffersBoughtNum.Find(x => x.OfferId == offer.Id).BoughtNum;

        return (offer.StartPrice + offer.PriceIncreaseAfterPurchase * howMuchTimesWasBought) * mult;
    }

    private void ClearPositions()
    {
        foreach (var pos in positions)
        {
            Destroy(pos.gameObject);
        }
        positions.Clear();
    }

    private void UpdateAvailableOffers()
    {
        availableOffers.Clear();

        for (int i = 0; i < AllExistingOffers.Count; i++)
        {
            if (player.MaxAchivedMiningDeviceLevel >= AllExistingOffers[i].MaxAchivedMinerLevelRequired)
            {
                availableOffers.Add(AllExistingOffers[i]);
            }
        }

        if (availableOffers.Count > 0)
        {
            var adMiningDeviceLevel = availableOffers[availableOffers.Count - 1].MiningDeviceLevel + 1;
            var addOffer = AllExistingAdOffers.Find(offer => offer.MiningDeviceLevel == adMiningDeviceLevel);
            if (addOffer != null)
            {
                availableOffers.Add(addOffer);
            }
        }
    }

    public void Save()
    {
        Saver.Save(SaveNames.OffersBought, new OfferBoughtNumSave(OffersBoughtNum));
    }
    private void Load()
    {
        var offersBoughtNum = new List<OfferBoughtNum>();

        foreach (var offer in AllExistingOffers)
        {
            offersBoughtNum.Add(new OfferBoughtNum(offer.Id, 0));
        }

        OffersBoughtNum = Saver.Load(SaveNames.OffersBought, new OfferBoughtNumSave(offersBoughtNum)).OffersBoughtNum;
    }
}

[Serializable]
public class OfferBoughtNum
{
    [SerializeField]
    private int offerId;
    [SerializeField]
    private int boughtNum;

    public int OfferId
    {
        get => offerId;
    }
    public int BoughtNum
    {
        get => boughtNum;
    }

    public OfferBoughtNum(int offerId, int boughtNum)
    {
        this.offerId = offerId;
        this.boughtNum = boughtNum;
    }

    public void IncreaseBoughtNum()
    {
        boughtNum++;
    }
}

[Serializable]
public class OfferBoughtNumSave
{
    [SerializeField]
    public List<OfferBoughtNum> OffersBoughtNum;

    public OfferBoughtNumSave(List<OfferBoughtNum> offersBoughtNum)
    {
        OffersBoughtNum = offersBoughtNum;
    }
}