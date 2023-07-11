using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipSource : MonoBehaviour
{
    public float myVolume;
    AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        myAudioSource.volume = myVolume;
    }
    public void PlaySourceClip(AudioClip myClip)
    {
        myAudioSource.clip = myClip;
        myAudioSource.Play();
    }

}
