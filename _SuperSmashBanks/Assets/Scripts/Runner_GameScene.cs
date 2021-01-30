using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_GameScene : MonoBehaviour
{
    // Bounds are (-6.5,-5) and (6.5,2)
    public static Vector2 playAreaLowerLeft = new Vector2(-6.5f, -5f);
    public static Vector2 playAreaUpperRight = new Vector2(6.5f, 2f);
    public static float BattlerHalfWidth = 0.25f;
    public static float tooCloseSqrMag = 0.25f;


    Color colorShort = new Color32(0,0,128,255);
    Color colorLong = new Color32(128,0,0,255);
    Color colorPlayerShort = new Color32(0,0,255,255);
    Color colorPlayerLong = new Color32(255,0,0,255);

    [SerializeField] Battler templateBattler;

    public int numStartingShortAIs;
    public int numStartingLongAIs;

    // Player is included
    public List<Battler> shorts;
    public List<Battler> longs;

    // Start is called before the first frame update
    void Start()
    {
        shorts = new List<Battler>();
        longs = new List<Battler>();
        InstantiateFactionAIs(Faction.Shorts, numStartingShortAIs);
        InstantiateFactionAIs(Faction.Longs, numStartingLongAIs);
    }

    void InstantiateFactionAIs(Faction faction, int number) {
        for (int i=0; i<number; i++) {
            var battler = InstantiateBattler(faction, true);
        }
    }

    Battler InstantiateBattler(Faction faction, bool isAI) {
        var battler = Instantiate(templateBattler);
        battler.bodyGraphic.color = (faction == Faction.Shorts) ? colorShort : colorLong;
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

    // Update is called once per frame
    void Update()
    {
        
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
}
