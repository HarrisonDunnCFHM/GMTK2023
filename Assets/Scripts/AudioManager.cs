using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> musicTracks;
    public float fadeTime = 1f;
    public float maxVolume = 0.3f;

    public float elapsedFade = 0f;

    [SerializeField] GameObject myAudioSourcePrefab;

    List<ClipSource> activeTracks;
    public List<ClipSource> tracksToFade;

    public ClipSource currentActiveTrack;
    public ClipSource nextActiveTrack;


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
    private void Update()
    {
        FadeMusic();
    }

    public void StartMusic()
    {
        activeTracks = new();
        tracksToFade = new();
        elapsedFade = fadeTime;
        for (int track = 0; track < musicTracks.Count; track++)
        {
            GameObject newAudio = Instantiate(myAudioSourcePrefab, transform.position, Quaternion.identity, transform);
            ClipSource newSource = newAudio.GetComponent<ClipSource>();
            newAudio.GetComponent<AudioSource>().clip = musicTracks[track];
            newAudio.GetComponent<AudioSource>().Play();
            newSource.name = newAudio.GetComponent<AudioSource>().clip.name;
            if (track > 0) { newSource.myVolume = 0f; }
            else { newSource.myVolume = maxVolume; tracksToFade.Add(newSource); }
            activeTracks.Add(newSource);
        }
    }

    public void InitiateMusicFade(int track)
    {
        //if (!tracksToFade.Contains(activeTracks[track]))
        //{

            tracksToFade.Add(activeTracks[track]);
        if (elapsedFade >= fadeTime)
        {
            elapsedFade = 0f;
        }
            //nextActiveTrack = activeTracks[track];
            //Debug.Log("switching track from " + tracksToFade[1].name + " to " 
            //    + tracksToFade[0].name);
           
        //}
        //else { return; }
    }

    private void FadeMusic()
    {
        if(tracksToFade.Count > 1)
        //if (nextActiveTrack != currentActiveTrack)
        {
            if (tracksToFade[0] == tracksToFade[1])
            {
                tracksToFade.RemoveAt(0);
                return;
            }
            if (elapsedFade < fadeTime)
            {
                tracksToFade[1].myVolume = Mathf.Lerp(0f, maxVolume, elapsedFade / fadeTime);
                tracksToFade[0].myVolume = Mathf.Lerp(maxVolume, 0f, elapsedFade / fadeTime);
                elapsedFade += Time.deltaTime;
            }
            else
            {
                tracksToFade[0].myVolume = 0f;
                tracksToFade[1].myVolume = maxVolume;
                tracksToFade.RemoveAt(0);
                elapsedFade = 0;

                //currentActiveTrack = nextActiveTrack;

            }
        }
        else { return; }
    }
}
