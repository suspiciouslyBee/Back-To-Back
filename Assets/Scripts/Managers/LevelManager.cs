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

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Makes into *local/scene-specific
    public static LevelManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
