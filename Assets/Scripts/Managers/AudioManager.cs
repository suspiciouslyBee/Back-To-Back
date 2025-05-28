using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    protected static AudioManager AMInstance;
    public static AudioManager Instance { get { return AMInstance; } }
    private AudioSource audSource;
    private bool FunkyMode;
    private float funk;
    private bool initialized = false;
    //------------------------- Getters -------------------------
    //------------------------- Setters -------------------------
    public void Awake()
    {
        InitAudioManager();
        /*
        audSource = GetComponent<AudioSource>();
        funk = 0.0f;
        FunkyMode = true;
        */
    }
    public void Update()
    {
        if (FunkyMode)
        {
            funk += 0.0001f;
            if (funk >= 3.0f)
            {
                funk = 0.0001f;
            }
            audSource.pitch = funk;
        }
        else
        {
            audSource.pitch = 1.0f;
        }
    }
    public void StopAllSounds()
    {
        /*
        Stop all sounds this Audio Manger is playing

        Inputs:
        * None

        Output:
        * None
        */
        audSource.Stop();
    }
    public void EnabledFunckyMode()
    {
        FunkyMode = true;
    }
    public void DisableFunckyMode()
    {
        FunkyMode = false;
    }
    //------------------------- Actions -------------------------
    public void PlayAudio(AudioClip audClip, float vol = 0.5f)
    {
        /*
        Play an Audio clip

        Inputs:
        * audClip: the audio clip to play
        * vol: the volume the audio clip plays at. Defaults to 0.5

        Output:
        * None
        */
        audSource.PlayOneShot(audClip, vol);
    }

    virtual public void InitAudioManager()
    {
        if (AMInstance != null && AMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            AMInstance = this;
            DontDestroyOnLoad(gameObject);
            initialized = true;
        }

        audSource = GetComponent<AudioSource>();
        funk = 0.0f;
        // FunkyMode = true;
    }
}
