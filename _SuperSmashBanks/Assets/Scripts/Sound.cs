using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    AudioClip audioClip;
    bool shouldOneShot;
    AudioSource audioSource;

    public Sound(AudioClip audioClip, bool shouldOneShot, AudioSource audioSource) {
        this.audioClip = audioClip;
        this.shouldOneShot = shouldOneShot;
        this.audioSource = audioSource;
    }

    public void Play() {
        //Debug.Log("Play was called");
        if (shouldOneShot) {
            audioSource.PlayOneShot(audioClip);
        } else {
            //audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
