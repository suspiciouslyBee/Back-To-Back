using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public void Update()
    {   
        // Temporay pause buttons to test. REMOVE ONCE PAUSE SCREEN IS IMPLEMENTED!!!
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            UnpauseGame();
        }
    }
    //------------------------- Getters -------------------------
    public bool IsGamePaused()
    {
        /*
        Get if the game is paused

        Inputs:
        * None

        Output:
        * True: game is paused
        * False: game is not paused
        */
        return isPaused;
    }
    //------------------------- Setters -------------------------
    public void PauseGame()
    {
        /*
        Pause the game by setting the time scale to 0 and diplaying the pause screen

        Inputs:
        * None

        Output:
        * None
        */
        isPaused = true;
        Time.timeScale = 0;
        DisplayPauseMenu();
        Debug.Log("Paused Game");
    }

    public void UnpauseGame()
    {
        /*
        Unpause the game by setting the time scale to 1 and hiding the pause screen

        Inputs:
        * None

        Output:
        * None
        */
        isPaused = false;
        HidePauseMenu();
        Time.timeScale = 1;
    }
    //------------------------- Actions -------------------------
    private void DisplayPauseMenu()
    {
        /*
        Display the pause screen. Nothing should require timescale to be greater than 0.

        Inputs:
        * None

        Output:
        * None
        */
    }

    private void HidePauseMenu()
    {
        /*
        Hide the pause screen.

        Inputs:
        * None

        Output:
        * None
        */
    }
}
