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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    void ChangeStageRelatively(int number)
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
        GUI.Label(new Rect(10, 10, 100, 50), "Controls:");
        GUI.Label(new Rect(10, 60, 100, 50), $"A/x: Right Attack");
        GUI.Label(new Rect(10, 110, 100, 50), $"S/y: Left Attack");
        GUI.Label(new Rect(10, 160, 100, 50), $"F/b: Reload");
        GUI.Label(new Rect(10, 210, 100, 50), $"SPACE/a: Swap");
        GUI.Label(new Rect(10, 260, 100, 50), $"R/start: Restart");
    }
}

//Stub GlobalVariables
//May consider for 1.1 some inter-session data persistence so I want to keep this
//done right
public class GlobalVariables
{
    public int foo;
}
