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
    [SerializeField] private UIDocument gameOverDoc;
    [SerializeField] private UIDocument pauseMenuDoc;
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

    }

    public void TogglePause()
    {
        paused = !paused;

        // hide/show pause menu
        // if (paused)
        // {
        //     gameOverUI.style.opacity = 100f;
        // }
        // else
        // {
        //     gameOverUI.style.opacity = 0;
        // }

    }

    // prepares all variables
    private void InitAll()
    {
        InitGameOver();
        InitPause();
    }

    // handles GameOver document initialization
    private void InitGameOver()
    {
        if (gameOverDoc != null)
        {
            gameOverUI = gameOverDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        }
        else
        {
            throw new NullReferenceException("No Game Over UI Document found in scene!");
        }


    }

    private void InitPause()
    {
        if (pauseMenuDoc != null)
        {
            pauseMenuUI = gameOverDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        }
        else
        {
            throw new NullReferenceException("No Pause Over UI Document found in scene!");
        }


    }

    // private UI elements
    private VisualElement gameOverUI;
    private VisualElement pauseMenuUI;

}
