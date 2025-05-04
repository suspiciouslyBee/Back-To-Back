using System;
using UnityEngine;
using UnityEngine.UIElements;
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
    public static UIManager Instance { get { return UIMInstance; } }

    private bool paused;
    private void Awake()
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
    }




    // animate the GameOver screen
    public void GameOverSequence()
    {
        gameOverUIScript.GameOverSequence();
    }

    public void TogglePause()
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
    private void InitAll()
    {
        gameOverUIScript.InitGameOver();
        //InitPause();
    }



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

}
