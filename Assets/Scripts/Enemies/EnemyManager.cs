using System.Collections.Generic;
using UnityEngine;




// Singleton pattern
public class EnemyManager : MonoBehaviour
{

    // public GameManager gameManager
    private static EnemyManager _instance;

    public GameObject[] enemyTypes;                                     // holds Prefabs of different enemies

    public static EnemyManager Instance { get { return _instance; } }

    public List<GameObject> enemies;
    public List<Enemy> enemyScripts;

    public List<Vector2> spawnPoints;

    [SerializeField] private float interval = 3f;
    private float curTime;

    const float fixedUpdateTime = 1 / 60f;

    private void Awake()
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
    }

    void Start()
    {
        Time.fixedDeltaTime = fixedUpdateTime;
    }

    // Remove Update as needed
    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > interval)
        {
            SpawnEnemy();
            curTime = 0f;
        }
    }

    // tick 60 times per second
    private void FixedUpdate()
    {
        foreach (Enemy enemy in enemyScripts)
        {
            enemy.AI();
            enemy.CheckIfGrounded();
            enemy.VisualUpdate();
        }
    }

    // spawn an enemy somewhere idk
    public void SpawnEnemy()
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
    public void SpawnEnemyType(int index, Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyTypes[index]);

        newEnemy.transform.position = position;

        enemies.Add(newEnemy);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScripts.Add(enemyScript);
        enemyScript.Spawn();
    }

    // stop tracking this enemy
    public void LoseEnemy(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        enemyScripts.Remove(enemyScript);
        enemies.Remove(enemy);
    }

}
