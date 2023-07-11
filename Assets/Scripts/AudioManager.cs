using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> musicTracks;
    public float fadeTime = 1f;
    public float maxVolume = 0.3f;

    [SerializeField] GameObject myAudioSourcePrefab;

    List<ClipSource> activeTracks;

    ClipSource currentActiveTrack;
    ClipSource nextActiveTrack;


    private void Awake()
    {
        AudioManager[] allAudio = FindObjectsOfType<AudioManager>();
        if(allAudio.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        StartMusic();
    }

    public void StartMusic()
    {
        activeTracks = new();

        for (int track = 0; track < musicTracks.Count; track++)
        {
            GameObject newAudio = Instantiate(myAudioSourcePrefab, transform.position, Quaternion.identity, transform);
            ClipSource newSource = newAudio.GetComponent<ClipSource>();
            newAudio.GetComponent<AudioSource>().clip = musicTracks[track];
            newAudio.GetComponent<AudioSource>().Play();
            if (track > 0) { newSource.myVolume = 0f; }
            else { newSource.myVolume = maxVolume;  currentActiveTrack = newSource; }
            activeTracks.Add(newSource);
        }
    }

    public void FadeToTrack(int track)
    {
        if (activeTracks[track] != currentActiveTrack)
        {
            nextActiveTrack = activeTracks[track];
            Debug.Log("switching track from " + currentActiveTrack.GetComponent<AudioSource>().clip.name + " to " 
                + activeTracks[track].GetComponent<AudioSource>().clip.name);
            
            nextActiveTrack.myVolume = maxVolume;
            currentActiveTrack.myVolume = 0f;
            currentActiveTrack = nextActiveTrack;
        }
        else { return; }
    }
}
