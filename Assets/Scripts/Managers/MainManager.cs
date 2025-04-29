/*
 * Main Manager
 * 
 * Description: Main Manager is the global singleton that mainly handles inter-level data and logic
*/


using UnityEditor.PackageManager;
using UnityEngine;

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
}

//Stub GlobalVariables
//May consider for 1.1 some inter-session data persistence so I want to keep this
//done right
public class GlobalVariables {
    public int foo;
}