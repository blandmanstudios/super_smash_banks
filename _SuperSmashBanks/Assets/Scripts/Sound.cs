using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    AudioClip audioClip;
    bool shouldOneShot;
    SoundMgr soundMgr;

    public Sound(AudioClip audioClip, bool shouldOneShot, SoundMgr soundMgr) {
        this.audioClip = audioClip;
        this.shouldOneShot = shouldOneShot;
        this.soundMgr = soundMgr;
    }

    public void Play() {
        //Debug.Log("Play was called");
        if (shouldOneShot) {
            if (!soundMgr.isImportantSoundPlaying) {
                soundMgr.audioSource.PlayOneShot(audioClip);
            }
        } else {
            //soundMgr.audioSource.Stop();
            soundMgr.audioSource.clip = audioClip;
            soundMgr.audioSource.Play();
            soundMgr.isImportantSoundPlaying = true;
        }
    }
}
