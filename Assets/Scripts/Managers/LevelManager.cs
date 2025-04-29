/*
 * Level Manager
 * 
 * Description: The Level Manager (LM) is a scene-specific singleton that interfaces between MM and
 * it's enemy
 * 
 * The idea is that the LM allows certain levels to have custom logic by promoting decoupling. The 
 * MM shouldn't have to khow each flavor of LM should work; it can manage itself largely? Also,
 * allows each level to specify special/tailored Enemy Managers (EM) if desired.
 * 
 * For each level, populate the prefab with an EM
*/

using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Makes into local/scene-specific singleton
    public static LevelManager Instance;

    //The extra checks are here incase there is a duplicate by any means
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


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
}
