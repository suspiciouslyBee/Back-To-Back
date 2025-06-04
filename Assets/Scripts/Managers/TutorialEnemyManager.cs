using UnityEngine;
using System.Collections.Generic;

public class TutorialEnemyManager : EnemyManager
{

    private float curTime;
    private new void Awake()
    {
        InitEnemyManager();
    }

    void Start()
    {
        Time.fixedDeltaTime = fixedUpdateTime;
    }

    // Remove Update as needed
    private new void Update()
    {
        if (interval != 0)
        {
            curTime += Time.deltaTime;
            if (curTime > interval)
            {
                SpawnEnemy();
                curTime = 0f;
            }
        }
    }

    // tick 60 times per second
    private new void FixedUpdate()
    {
        foreach (Enemy enemy in enemyScripts)
        {
            enemy.AI();
            enemy.CheckIfGrounded();
            enemy.VisualUpdate();
        }
    }

    override public int GetEnemyCount()
    {
        return enemies.Count;
    }

    // spawn an enemy somewhere idk
    override public void SpawnEnemy()
    {
        int type;
        Vector2 position;
        if (enemyTypes.Length > 0)
        {
            // choose random enemy type for now
            type = Random.Range(0, enemyTypes.Length);

            if (spawnPoints.Count > 0)
            {
                position = spawnPoints[Random.Range(0, spawnPoints.Count)];
            }
            else
            {
                // spawn at 0,1
                position = new(0, 1);
            }
            // Debug.Log($"Spawning enemy of type {type}");
            SpawnEnemyType(type, position);
        }

        // Debug.Log("No enemy types available");
    }

    // spawn a specific enemy from an index
    override public void SpawnEnemyType(int index, Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyTypes[index]);

        newEnemy.transform.position = position;

        enemies.Add(newEnemy);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScripts.Add(enemyScript);
        enemyScript.Spawn();
    }

    override public void InitEnemyManager()
    {
        // maintain the singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        curTime = 0f;
        initialized = true;
    }

    override public void AdjustSpawning(float newTiming)
    {
        interval = newTiming;
    }
}

