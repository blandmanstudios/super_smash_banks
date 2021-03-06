﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_GameScene : MonoBehaviour
{
    // Testing switches
    private bool isStockPriceVariable = true;

    public static Vector2 playAreaLowerLeft = new Vector2(-6.65f, -5f);
    public static Vector2 playAreaUpperRight = new Vector2(6.65f, 2f);
    public static float BattlerHalfWidth = 0.25f;
    public static float tooCloseSqrMag = 0.25f;

    private float openingStockPrice = 550f;
    private float stockPriceFloor = 100f;
    private float stockPriceCeiling = 999.99f;
    private float previousStockPrice;
    private static float stockPrice;
    public static float StockPrice => stockPrice;

    float timeStockPriceLastSet = float.MinValue;
    float timeBetweenPriceUpdates = 1f;

    public static float playerMoveSpeed = 4f;
    public static float aiMoveSpeed = 3.5f;

    // Shorts wear blue!
    Color colorShort = new Color32(0,0,128,255);
    Color colorLong = new Color32(128,0,0,255);
    Color colorPlayerShort = new Color32(0,0,255,255);
    Color colorPlayerLong = new Color32(255,0,0,255);
    Color colorMeleeShort = new Color32(0,0,255,128);
    Color colorMeleeLong = new Color32(255,0,0,128);

    [SerializeField] Battler templateBattler;
    [SerializeField] Melee templateMelee;
    [SerializeField] Stock templateStock;

    public static int numStartingShortAIs = 10;
    public static int numStartingLongAIs = 10;

    // Player is included
    public List<Battler> shorts;
    public List<Battler> longs;

    public CharSpriteMgr charSpriteMgr;
    public UIController uiController;
    [SerializeField] GameObject folderBattlers;

    public SoundMgr soundMgr;

    // Don't set this directly. Use the setting function.
    bool waitingForPlayerEntry;

    // Start is called before the first frame update
    void Start()
    {
        DoOncePerSession();
        DoBeforeEachPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStockPriceLastSet + timeBetweenPriceUpdates < Time.time) {
            if (isStockPriceVariable) {
                PickAndSetNewStockPrice();
            }
            uiController.UpdateStockPriceDisplay();
            timeStockPriceLastSet = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            CleanUpAfterPlay();
            DoBeforeEachPlay();
        }

        if (waitingForPlayerEntry) {
            if (Input.GetKeyDown(KeyCode.A)) {
                SetWaitingForPlayerEntry(false);
                InstantiateBattler(Faction.Shorts, false);
                soundMgr.Stop();
                soundMgr.soundPlayerEntersTheFray.Play();
            } else if (Input.GetKeyDown(KeyCode.D)) {
                SetWaitingForPlayerEntry(false);
                InstantiateBattler(Faction.Longs, false);
                soundMgr.Stop();
                soundMgr.soundPlayerEntersTheFray.Play();
                
            }
        }
    }

    void DoOncePerSession() {

    }

    void DoBeforeEachPlay() {
        uiController.ShowYouLosePanel(false);
        uiController.HideGotOutPanel();

        // Must happen before battlers are created
        stockPrice = openingStockPrice;
        previousStockPrice = openingStockPrice;

        shorts = new List<Battler>();
        longs = new List<Battler>();
        InstantiateFactionAIs(Faction.Shorts, numStartingShortAIs);
        InstantiateFactionAIs(Faction.Longs, numStartingLongAIs);
        SetWaitingForPlayerEntry(true);

        soundMgr.soundGameStart.Play();
        // TODO: enforce a join timer
        soundMgr.soundRunningOutOfTimeToJoin.Play();
     }

    void CleanUpAfterPlay() {
        DestroyBattlersInList(shorts);
        DestroyBattlersInList(longs);
        soundMgr.Stop();
    }

    void DestroyBattlersInList(List<Battler> battlers) {
        for(int i=battlers.Count - 1; i>=0; i--) {
            var currentBattler = battlers[i];
            battlers.RemoveAt(i);
            Destroy(currentBattler.gameObject);
        }
    }

    void InstantiateFactionAIs(Faction faction, int number) {
        for (int i=0; i<number; i++) {
            var battler = InstantiateBattler(faction, true);
        }
    }

    Battler InstantiateBattler(Faction faction, bool isAI) {
        var battler = Instantiate(templateBattler);
        battler.InitStartingValues();
        battler.gameObject.transform.SetParent(folderBattlers.transform);
        
        if (isAI) {
            //battler.bodyGraphic.color = (faction == Faction.Shorts) ? colorShort : colorLong;
        } else {
            battler.bodyGraphic.color = (faction == Faction.Shorts) ? colorPlayerShort : colorPlayerLong;
        }

        battler.faction = faction;
        switch (faction) {
            case Faction.Shorts:
                shorts.Add(battler);
                break;
            case Faction.Longs:
                longs.Add(battler);
                break;
            default:
                throw new System.Exception($"Unhandled faction {faction}");
        }
        battler.isAI = isAI;
        battler.isBattlerActive = true;

        if (isAI) {
            battler.InitAndStartAIMovement();
            battler.InitAndStartAIAttacking();
        }

        // TODO: We could use a singleton pattern

        battler.runner = this;

        // TODO: This could fail if the arena is too small for the number of battlers

        Vector2 candidateLocation = Vector2.zero;
        bool stillLookingForStartingLocation = true;
        int failsafe = 0;
        int tooManyTries = 100;
        while (stillLookingForStartingLocation && failsafe < tooManyTries) {
            failsafe++;
            candidateLocation = PickCandidateSpawnLocation();
            if (!IsLocationOccupied(candidateLocation)) {
                stillLookingForStartingLocation = false;
            }
        }
        if (stillLookingForStartingLocation) {
            throw new System.Exception("RNG failed to find a starting location for us");
        } else {
            battler.transform.position = candidateLocation;
            return battler;
        }
    }

    public Vector2 PickCandidateSpawnLocation() {
        float randX = Random.Range(playAreaLowerLeft.x + BattlerHalfWidth*2,
            playAreaUpperRight.x - BattlerHalfWidth*2);
        float randY = Random.Range(playAreaLowerLeft.y + BattlerHalfWidth*2,
            playAreaUpperRight.y - BattlerHalfWidth*2);
        var candidate = new Vector2(randX, randY);
        return candidate;
    }

    public bool IsLocationOccupied(Vector2 candidateLocation) {
        return (IsLocationOccupiedBySomeoneFromList(candidateLocation, shorts) ||
            IsLocationOccupiedBySomeoneFromList(candidateLocation, longs));
    }

    public bool IsLocationOccupiedBySomeoneFromList(Vector2 candidateLocation, List<Battler> battlers) {
        foreach(var battler in battlers) {
            var dist = (Vector2)battler.transform.position - candidateLocation;
            var distSquared = Vector2.SqrMagnitude(dist);
            if (distSquared < tooCloseSqrMag) {
                return true;
            }
        }
        return false;
    }

    public Melee InstantiateMelee(Vector2 location, Faction faction) {
        var newMelee = Instantiate(templateMelee, location, Quaternion.identity);
        newMelee.bodyGraphic.color = (faction == Faction.Shorts) ? colorMeleeShort : colorMeleeLong;
        newMelee.faction = faction;
        soundMgr.soundAnyCharacterAttacks.Play();
        return newMelee;
    }

    public Stock InstantiateStock(Vector2 location) {
        //Debug.Log("Stock instantiated");
        var newStock = Instantiate(templateStock, location, Quaternion.identity);
        return newStock;
    }

    private void PickAndSetNewStockPrice() {
        previousStockPrice = stockPrice;
        stockPrice = Random.Range(stockPriceFloor, stockPriceCeiling);
        if (stockPrice > previousStockPrice) {
            soundMgr.soundStockPriceUp.Play();
        } else if (stockPrice < previousStockPrice) {
            // Can't just use Else because they could be equal 
            soundMgr.soundStockPriceDown.Play();
        }
    }

    public static float DollarsToShares(float dollars) {
        if (stockPrice != 0) {
            var shares = dollars / stockPrice;
            return shares;
        } else {
            throw new System.Exception("Tried to calculate DollarsToShares() with stockPrice of 0");
        }
    }

    public static float SharesToDollars(float shares) {
        if (stockPrice != 0) {
            var dollars = shares * stockPrice;
            return dollars;
        } else {
            throw new System.Exception("Tried to calculate SharesToDollars() with stockPrice of 0");
        }
    }

    void SetWaitingForPlayerEntry(bool value) {
        waitingForPlayerEntry = value;
        if (waitingForPlayerEntry) {
            uiController.SetRightPanelToJoinInstructions();
            uiController.UpdatePlayerStatsDisplay(null);
        } else {
            uiController.SetRightPanelToGetOutInstructions();
        }
    }
}
