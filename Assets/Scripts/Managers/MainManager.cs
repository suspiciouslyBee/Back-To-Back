/*
 * Main Manager
 * 
 * Description: Main Manager is the global singleton that mainly handles inter-level data and logic
*/


using System;
using UnityEditor.PackageManager;
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
        if(LevelManager.LMInstance == null)
        {
            throw new NullReferenceException("Scene has no Level Manager!");
        }

        return LevelManager.LMInstance;
    }

    public void RestartLevel()
    {
        ReloadStage();
    }
    void ChangeStage(int number)
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

    void PreviousStage()
    {
        ChangeStage(-1);
    }

    void NextStage()
    {
        ChangeStage(1);
    }

    void ReloadStage()
    {
        ChangeStage(0);
    }
}

//Stub GlobalVariables
//May consider for 1.1 some inter-session data persistence so I want to keep this
//done right
public class GlobalVariables {
    public int foo;
}