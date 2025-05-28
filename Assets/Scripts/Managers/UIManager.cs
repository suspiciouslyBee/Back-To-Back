using System;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(AudioSource))]
/* 
    UIManager
    Description:
        Manages all UI documents within the game.
        

    Singleton implementation based on:
        https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
*/
public class UIManager : MonoBehaviour
{
    private static UIManager UIMInstance;

    [SerializeField] private UIDocument pauseMenuDoc;

    [SerializeField] private GameOverUI gameOverUIScript;

    [SerializeField] private AudioClip buttonClickSFX;

    protected AudioSource audioSource;
    public static UIManager Instance { get { return UIMInstance; } }

    private bool paused;

    public bool initialized = false;
    void Awake()
    {
        InitUIManager();
    }




    // animate the GameOver screen
    public virtual void GameOverSequence()
    {
        gameOverUIScript.GameOverSequence();
    }

    public virtual void TogglePause()
    {
        paused = !paused;

        // hide/show pause menu
        if (paused)
        {
            pauseMenuDoc.rootVisualElement.style.display = DisplayStyle.None;
        }
        else
        {
            pauseMenuDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        }

    }

    // prepares all variables
    protected virtual void InitAll()
    {
        gameOverUIScript.InitGameOver();
        audioSource = GetComponent<AudioSource>();
        //InitPause();
    }


    // move to PauseUI.cs when it's created
    private void InitPause()
    {
        if (pauseMenuDoc != null)
        {
            pauseMenuUI = pauseMenuDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        }
        else
        {
            throw new NullReferenceException("No Pause Over UI Document found in scene!");
        }


    }

    // private UI elements

    private VisualElement pauseMenuUI;

    // does whatever Awake() would do
    public void InitUIManager()
    {
        if (UIMInstance != null && UIMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            UIMInstance = this;
        }
        InitAll();
        paused = false;
        initialized = true;
    }

    public void PlayButtonClickSFX()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        AudioManager.Instance.PlayAudio(buttonClickSFX, 0.08f);
    }

}
