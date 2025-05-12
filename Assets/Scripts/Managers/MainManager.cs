/*
 * Main Manager
 * 
 * Description: Main Manager is the global singleton that mainly handles inter-level data and logic
*/


using System;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{

    //Makes into *global* singleton
    public static MainManager Instance;

    GUIStyle debugStyle = new GUIStyle();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        debugStyle.fontSize = 30;
        debugStyle.normal.textColor = Color.white;
    }


    public LevelManager GetLevelManager()
    {
        if (LevelManager.LMInstance == null)
        {
            throw new NullReferenceException("Scene has no Level Manager!");
        }

        return LevelManager.LMInstance;
    }

    //bounds checking to change the stage relative to current
    public void ChangeStageRelatively(int number)
    {
        int newIndex = SceneManager.GetActiveScene().buildIndex + number;

        //bounds check
        if (0 > newIndex || newIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("No more scenes!");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + number);
    }

    //wrapper/macro for restarting the level by reloading the scene
    public void RestartLevel()
    {
        ChangeStageRelatively(0);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 160, 100, 50), "Controls:", debugStyle);
        GUI.Label(new Rect(10, 210, 100, 50), $"A/x: Melee Attack", debugStyle);
        GUI.Label(new Rect(10, 260, 100, 50), $"S/y: Ranged Attack", debugStyle);
        GUI.Label(new Rect(10, 310, 100, 50), $"F/b: Reload", debugStyle);
        GUI.Label(new Rect(10, 360, 100, 50), $"SPACE/a: Swap", debugStyle);
        GUI.Label(new Rect(10, 410, 100, 50), $"R/start: Restart", debugStyle);
    }
}

//Stub GlobalVariables
//May consider for 1.1 some inter-session data persistence so I want to keep this
//done right
public class GlobalVariables
{
    public int foo;
}
