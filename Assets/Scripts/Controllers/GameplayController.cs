using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    private Player player;
    public GameRules GameRules;
    
    public MiningController miningController;
    public GameplayHelper GameplayHelper;
    public Box Box;
    public Roulette Roulette;
    public FlyingBonusController FlyingBonusController;
    private FlyingBonusHandler flyingBonusHandler;
    public BoostController BoostController;
    public UpgradeShopController UpgradeShopController;
    public MiningDeviceShop MiningDeviceShop;
    public TrashCan TrashCan;
    public MiningDeviceChooser miningDeviceChooser;
    public Gift Gift;
    public DiamondPurchaseAnimation DiamondPurchaseAnimation;
    public Subscription Subscription;
    public PurchaseInitialiser PurchaseInitialiser;


    public Text DEBUGTEXT;

    [Header("Shop")]
    public Shop Shop;
    public PurchaseHandler PurchaseHandler;

    [Header("Relocation")]
    public RelocationPanel RelocationPanel;
    public Relocator Relocator;

    [Header("Field")]
    public MergeFieldBuilder MergeFieldBuilder;
    private MergeFieldFiller mergeFieldFiller;
    private OperationsOnField operationsOnField;
    private CellHighlighter cellHighlighter;

    [Header("Devices")]
    public MiningDeviceGenerator MiningDeviceGenerator;
    public MiningDeviceMerger MiningDeviceMerger;
    public MiningDevicePlacer miningDevicePlacer;

    [Header("Raycasting")]
    public Canvas Canvas; 
    public Transform BufferForDraggedItem; 
    public EventSystem EventSystem;

    [Header("UI")]
    public WindowController WindowController;
    public ButtonBinder ButtonBinder;
    public List<PlayerResourceUI> resourceUIs;
    public WheelAttemptsUI WheelAttemptsUI;
    public SubscriptionButtonView SubscriptionButtonView;

    [Header("Animation")]
    public Jumper Jumper;
    public RelativePositionsCalculator RelativePositionsCalculator;
    public NewMiningDeviceAnimation NewMiningDeviceAnimation;

    [Header("Indicators")]
    public FillIndicatorUI GiftIndicator;
    public ButtonHighlight GiftHighlight;
    public FillIndicatorUI RelocationIndicator;
    public ButtonHighlight RelocationHighlight;

    [Header("Ads")]
    public Ads Ads;

    [Header("Wheel")]
    public Wheel Wheel;

    private List<ISavable> savables = new List<ISavable>();

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Initialization();
    }

    private void Initialization()
    {
        InitPlayer();

        InitPurchaseInitialiser();
        InitSubscription();

        InitMergeFieldBuilder();
        InitMiningDevicePlacer();
        InitOperationsOnField();
        InitCellHighlighter();
        InitTrashCan();
        InitMiningDeviceGenerator();
        InitMiningDeviceMerger();
        InitResourcesUIs();
        InitMiningDeviceChooser();
        InitMergeFieldFiller();
        InitMiningController();
        InitBox();
        InitRelocator();
        InitRelocationPanel();
        InitBoostController();
        InitGameplayHelper();
        InitRoulette();
        InitFlyingBonusHandler();
        InitFlyingBonusController();
        InitUpgradeShopController();
        InitButtonBinder();
        InitMiningDeviceShop();
        InitJumper();
        InitGift();
        InitNewMinerAnimation();
        InitShop();
        InitPurchaseHandler();
        InitDiamondPurchaseAnimation();
        InitAds();
        InitWheel();
        InitWheelAttemptsUI();
        InitSubscriptionButtonView();
    }

    private void InitSubscriptionButtonView()
    {
        SubscriptionButtonView.Construct(Subscription);
    }

    private void InitWheelAttemptsUI()
    {
        WheelAttemptsUI.Construct(Wheel);
    }

    private void InitWheel()
    {
        Wheel.Construct(BoostController, WindowController);
        savables.Add(Wheel);
    }

    private void InitPurchaseInitialiser()
    {
        PurchaseInitialiser.Construct();
    }

    private void InitSubscription()
    {
        Subscription.Construct(PurchaseInitialiser);
    }

    private void InitDiamondPurchaseAnimation()
    {
        DiamondPurchaseAnimation.Construct(RelativePositionsCalculator, PurchaseHandler);
    }

    private void InitPurchaseHandler()
    {
        PurchaseHandler.Construct(player);
    }

    private void InitShop()
    {
        Shop.Construct();
    }

    private void InitAds()
    {
        Ads.Construct(Subscription);
    }

    private void InitNewMinerAnimation()
    {
        NewMiningDeviceAnimation.Construct(RelativePositionsCalculator, GameRules);
    }

    private void InitGift()
    {
        Gift.Construct(player, WindowController, GiftIndicator, GiftHighlight);
    }

    private void InitMiningDeviceChooser()
    {
        miningDeviceChooser = new MiningDeviceChooser(player, GameRules, Subscription);
    }

    private void InitTrashCan()
    {
        TrashCan.Construct();
    }

    private void InitJumper()
    {
        Jumper.Construct(RelativePositionsCalculator);
    }

    private void InitMiningDeviceShop()
    {

        MiningDeviceShop.Construct(player, GameRules, mergeFieldFiller, MergeFieldBuilder, Ads, Subscription);
        savables.Add(MiningDeviceShop);
    }

    private void InitRelocationPanel()
    {
        RelocationPanel.Construct(player, GameRules);
    }

    private void InitButtonBinder()
    {
        ButtonBinder.Construct(WindowController, Relocator, BoostController, mergeFieldFiller, Gift, Ads, Wheel, player);
    }

    private void InitUpgradeShopController()
    {
        UpgradeShopController.Construct(BoostController, player, Ads);
    }

    private void InitBoostController()
    {
        BoostController.Construct(miningController, Box, MergeFieldBuilder, player);
        savables.Add(BoostController);
    }

    private void InitFlyingBonusHandler()
    {
        flyingBonusHandler = new FlyingBonusHandler(player, WindowController);
    }

    private void InitFlyingBonusController()
    {
        FlyingBonusController.Construct(RelativePositionsCalculator, GameRules, flyingBonusHandler);
    }

    private void InitRoulette()
    {
        Roulette.Construct(player, GameRules);
    }

    private void InitGameplayHelper()
    {
        GameplayHelper.Construct(operationsOnField, RelativePositionsCalculator, Box);
    }
    private void InitOperationsOnField()
    {
        operationsOnField = new OperationsOnField(MergeFieldBuilder);
    }
    private void InitCellHighlighter()
    {
        cellHighlighter = new CellHighlighter(MergeFieldBuilder);
    }
    private void InitMiningController()
    {
        miningController.Construct(player, MergeFieldBuilder);
        savables.Add(miningController);
    }
    private void InitRelocator()
    {
        Relocator.Construct(MergeFieldBuilder, mergeFieldFiller, player, GameRules, RelocationIndicator, RelocationHighlight);
    }
    private void InitPlayer()
    {
        player = Saver.Load(SaveNames.Player, Player.GetStartPlayerSettings());
        savables.Add(player);
    }
    private void InitMergeFieldFiller()
    {
        mergeFieldFiller = new MergeFieldFiller(MergeFieldBuilder, miningDevicePlacer, MiningDeviceGenerator, GameRules, miningDeviceChooser);
        savables.Add(mergeFieldFiller);
    }
    private void InitBox()
    {
        Box.Construct(mergeFieldFiller, MergeFieldBuilder, GameplayHelper, WindowController, miningDeviceChooser);
    }
    private void InitResourcesUIs()
    {
        foreach (var resourceUI in resourceUIs)
        {
            resourceUI.Construct(player);
        }
    }
    private void InitMergeFieldBuilder()
    {
        MergeFieldBuilder.Construct(player, MiningDeviceMerger, GameRules, Subscription);
        MergeFieldBuilder.BuildMergeField();
    }
    private void InitMiningDeviceMerger()
    {
        MiningDeviceMerger.Construct(GameRules, player, WindowController);
    }
    private void InitMiningDeviceGenerator()
    {
        MiningDeviceGenerator.Construct(GameRules, Canvas, BufferForDraggedItem, 
            EventSystem, Jumper, MergeFieldBuilder, cellHighlighter, GameplayHelper,
            WindowController, Roulette, TrashCan);
    }
    private void InitMiningDevicePlacer()
    {
        miningDevicePlacer.Construct(MergeFieldBuilder, Jumper);
    }



    private void SaveAll()
    {
        foreach (var savable in savables)
        {
            savable.Save();
        }
    }
    private void OnApplicationQuit()
    {
        SaveAll();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveAll();
        }
    }
}
