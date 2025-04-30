/*
 * Level Manager
 * 
 * Description: The Level Manager (LM) is a scene-specific singleton that interfaces between MM and
 * it's enemy
 * 
 * The idea is that the LM allows certain levels to have custom logic by promoting decoupling. The 
 * MM shouldn't have to khow each flavor of LM should work; it can manage itself largely? Also,
 * allows each level to specify special/tailored Enemy Managers (EM) if desired.
 * Override example: It could be useful to override behavior for a non combat scene like a main menu
 * 
 * For each level, populate the prefab with an EM
 * 
 * this is rough. will need revision
*/

//TODO: make this not abstract once the PC is drafted

using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static MainManager Instance;

    //Makes into local/scene-specific singleton
    public static LevelManager LMInstance;

    public PlayerManager PCInstance;
    //The extra checks are here incase there is a duplicate by any means
    private void Awake()
    {
        if (LMInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        LMInstance = this;

        if (Instance == null)
        {
            Instance = MainManager.Instance;
        }


        //if the EM is not specified we will attempt to fill it
        if(enemyManager == null) {
            Debug.Log("Enemy Manager not explictly specified for this level!\nSearching...");
            enemyManager = EnemyManager.Instance;
        }

        //checks again and throws an exception if we still have nothing
        if(enemyManager == null) {
            throw new NullReferenceException("Can't find Enemy Manager for Scene!");
        }

    }

    
    [SerializeField] private EnemyManager enemyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        Instance.RestartLevel();
    }

    //running on assumptions
    //can be overridden by a class that inherits LM?
    
    public void FireGun()
    {
        PCInstance.Attack(false);
    }

    public void ReloadGun()
    {
        PCInstance.Reload();
    }

    public void SwingSword()
    {
        PCInstance.Attack(true);
    }


    public void SwapChars()
    {
        PCInstance.Swap();
    }


}
