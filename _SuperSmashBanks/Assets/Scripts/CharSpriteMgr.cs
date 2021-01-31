using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpriteMgr : MonoBehaviour
{
    // All battlers look here to determine which foot is forward
    public static AnimState animState;
    float timeInEachState = 0.25f;
    List<AnimState> animCycle;
    float timeForFullCycle;

    public Sprite blackLeftForward;
    public Sprite blackRightForward;
    public Sprite blackStationary;
    public Sprite blueLeftForward;
    public Sprite blueRightForward;
    public Sprite blueStationary;
    public Sprite redLeftForward;
    public Sprite redRightForward;
    public Sprite redStationary;

    public static Dictionary<AnimState, Sprite> redSprites;
    public static Dictionary<AnimState, Sprite> blueSprites;
    public static Dictionary<AnimState, Sprite> blackSprites;

    // Start is called before the first frame update
    void Start()
    {
        animCycle = new List<AnimState>();
        animCycle.Add(AnimState.Stationary);
        animCycle.Add(AnimState.LeftForward);
        animCycle.Add(AnimState.Stationary);
        animCycle.Add(AnimState.RightForward);

        redSprites = new Dictionary<AnimState, Sprite>();
        redSprites.Add(AnimState.LeftForward, redLeftForward);
        redSprites.Add(AnimState.RightForward, redRightForward);
        redSprites.Add(AnimState.Stationary, redStationary);

        blueSprites = new Dictionary<AnimState, Sprite>();
        blueSprites.Add(AnimState.LeftForward, blueLeftForward);
        blueSprites.Add(AnimState.RightForward, blueRightForward);
        blueSprites.Add(AnimState.Stationary, blueStationary);

        blackSprites = new Dictionary<AnimState, Sprite>();
        blackSprites.Add(AnimState.LeftForward, blackLeftForward);
        blackSprites.Add(AnimState.RightForward, blackRightForward);
        blackSprites.Add(AnimState.Stationary, blackStationary);

        // animCycle must be populated before you go on
        timeForFullCycle = timeInEachState * animCycle.Count;
    }

    // Update is called once per frame
    void Update()
    {
        var mod = Time.time % timeForFullCycle;
        Debug.Log($"mod: {mod}");
        for (int i=animCycle.Count-1; i>0; i--) {
            if(i*timeInEachState <= mod && mod < (i+1)*timeInEachState) {
                animState = animCycle[i];
                Debug.Log($"animState: {animState}");
                return;
            }
        }
        animState = animCycle[0];
        Debug.Log($"animState: {animState}");
    }
}
