using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJManager : MonoBehaviour
{
    private static DJManager DJInstance;
    public static DJManager Instance { get { return DJInstance; } }
    public bool initialized;

    AudioSource aS;
    [SerializeField] List<AudioClip> themes;
    public float volume;
    int trackNum;

    private void Awake()
    {
        InitDJManager();
    }

    // does whatever Awake would do
    private void InitDJManager()
    {
        if (DJInstance != null && DJInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            DJInstance = this;
            aS = gameObject.GetComponent<AudioSource>();
            PlayTrack(1);
        }
        initialized = true;

    }

    public void PlayTrack(int trackNumber)
    {
        switch(trackNumber)
        {
            case 1:
                StartCoroutine(PlayTheme(0, trackNumber));
                break;
            case 2:
                StartCoroutine(PlayTheme(1, trackNumber));
                break;
            case 3:
                StartCoroutine(PlayTheme(2, trackNumber));
                break;
        }
    }

    private IEnumerator PlayTheme(int trackLocation, int newTrack)
    {
        if (trackNum != newTrack)
        {
            float maxVolume = volume;
            if (aS.isPlaying)
            {
                for (int i = 0; i < 10; i++)
                {
                    aS.volume -= (maxVolume / 10);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            aS.volume = 0;
            aS.Stop();
            yield return new WaitForSeconds(0.25f);
            aS.clip = themes[trackLocation];
            aS.Play();
            trackNum = newTrack;
            for (int i = 0; i < 10; i++)
            {
                aS.volume += (maxVolume / 10);
                yield return new WaitForSeconds(0.05f);
            }
            volume = maxVolume;
            aS.volume = volume;
        }
    }


}
