using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] AudioClip gameStart;
    [SerializeField] AudioClip playerEntersTheFray;
    [SerializeField] AudioClip playerAcquiresStock;
    [SerializeField] AudioClip runningOutOfTimeToJoin;
    [SerializeField] AudioClip anyCharacterAttacks;
    [SerializeField] AudioClip stockPriceUp;
    [SerializeField] AudioClip stockPriceDown;
    [SerializeField] AudioClip badGameOver;
    [SerializeField] AudioClip aiAcquiresStock;
    [SerializeField] AudioClip aiHurt1;
    [SerializeField] AudioClip aiHurt2;
    [SerializeField] AudioClip aiHurt3;
    [SerializeField] AudioClip goodGameOver;
    [SerializeField] AudioClip playerHurt;

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

    List<Sound> soundsAiHurt;

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

        soundsAiHurt= new List<Sound>();
        soundsAiHurt.Add(soundAiHurt1);
        soundsAiHurt.Add(soundAiHurt2);
        soundsAiHurt.Add(soundAiHurt3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Stop() {
        audioSource.Stop();
    }

    public Sound PickAiHurtSound() {
        var indexSelected = Random.Range(0, soundsAiHurt.Count);
        Sound soundSelected = soundsAiHurt[indexSelected];
        return soundSelected;
    }
}
