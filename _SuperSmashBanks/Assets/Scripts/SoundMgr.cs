using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip gameStart;
    public AudioClip playerEntersTheFray;
    public AudioClip playerAcquiresStock;
    public AudioClip runningOutOfTimeToJoin;
    public AudioClip anyCharacterAttacks;
    public AudioClip stockPriceUp;
    public AudioClip stockPriceDown;
    public AudioClip badGameOver;
    public AudioClip aiAcquiresStock;
    public AudioClip aiHurt1;
    public AudioClip aiHurt2;
    public AudioClip aiHurt3;
    public AudioClip goodGameOver;
    public AudioClip playerHurt;

    public Sound soundGameStart;
    public Sound soundPlayerEntersTheFray;
    public Sound soundPlayerAcquiresStock;
    public Sound soundRunningOutOfTimeToJoin;
    public Sound soundAnyCharacterAttacks;
    public Sound soundStockPriceUp;
    public Sound soundStockPriceDown;
    public Sound soundBadGameOver;
    public Sound soundAiAcquiresStock;
    public Sound soundAiHurt1;
    public Sound soundAiHurt2;
    public Sound soundAiHurt3;
    public Sound soundGoodGameOver;
    public Sound soundPlayerHurt;


    void Awake()
    {
        // This is happening in Awake() because some sounds are called on Start().
        soundGameStart = new Sound(gameStart, true, audioSource);
        soundPlayerEntersTheFray = new Sound(playerEntersTheFray, true, audioSource);
        soundPlayerAcquiresStock = new Sound(playerAcquiresStock, true, audioSource);
        soundRunningOutOfTimeToJoin = new Sound(runningOutOfTimeToJoin, false, audioSource);
        soundAnyCharacterAttacks = new Sound(anyCharacterAttacks, true, audioSource);
        soundStockPriceUp = new Sound(stockPriceUp, true, audioSource);
        soundStockPriceDown = new Sound(stockPriceDown, true, audioSource);
        soundBadGameOver = new Sound(badGameOver, false, audioSource);
        soundAiAcquiresStock = new Sound(aiAcquiresStock, true, audioSource);
        soundAiHurt1 = new Sound(aiHurt1, true, audioSource);
        soundAiHurt2 = new Sound(aiHurt2, true, audioSource);
        soundAiHurt3 = new Sound(aiHurt3, true, audioSource);
        soundGoodGameOver = new Sound(goodGameOver, false, audioSource);
        soundPlayerHurt = new Sound(playerHurt, true, audioSource);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Stop() {
        audioSource.Stop();
    }
}
