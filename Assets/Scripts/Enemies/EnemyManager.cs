using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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

    [SerializeField] private readonly float initInterval = 5f;           // initial interval between waves
    [SerializeField] private float minInterval = 2f;            // absolute minimum value that interval can be

    public int wavesSpawned = 0;                                   // number of waves spawned

    [SerializeField] private readonly float waveSizeDeviation = 0.25f;     // wave_size = wave_size * (1 +- waveSizeDeviation)
    private float curTime;

    const float fixedUpdateTime = 1 / 60f;

    public bool initialized = false;

    [SerializeField] private float inWaveSpawnDelay = 0.1f;     // when spawning a group of zombies, produce a slight delay between
                                                                // spawns

    [SerializeField] private float waveSpawnTimeMult = 1f;      // controls how rapidly zombie waves spawn

    [SerializeField] private const int spawnCap = 50;           // maximum number of zombies allowed on screen

    public int totalEnemiesSpawned = 0;


    private float interval;

    private void Awake()
    {
        InitEnemyManager();
    }

    void Start()
    {
        Time.fixedDeltaTime = fixedUpdateTime;
    }

    // Remove Update as needed
    private void Update()
    {
        curTime += Time.deltaTime;
        Debug.Log($"total enemies spawned: {totalEnemiesSpawned}");
        if (curTime > interval)
        {
            int count = RandomEnemyCount(LevelManager.LMInstance.timeSurvived);
            (Vector2, Vector2) positions = PrimaryPosition();
            StartCoroutine(SpawnGroup((int)Mathf.Max(count * 0.8f, 1), positions.Item1));
            StartCoroutine(SpawnGroup((int)Mathf.Max(count * 0.2f - 1, 0), positions.Item2));
            // logic to change the interval?
            curTime = 0f;
            interval *= 1.25f;                            // wave interval increases over time
            wavesSpawned++;

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
        Vector2 position;
        if (spawnPoints.Count > 0)
        {
            position = spawnPoints[Random.Range(0, spawnPoints.Count)];
        }
        else
        {
            // spawn at 0,1
            position = new(0, 1);
        }
        SpawnEnemy(position);
    }

    // spawns an enemy at a position
    public void SpawnEnemy(Vector2 position)
    {
        if (enemies.Count >= spawnCap)
        {
            Debug.Log($"[EnemyManager] attempting to spawn enemy, but spawn cap reached!");
            return;
        }
        int type;

        if (enemyTypes.Length > 0)
        {
            // choose random enemy type for now
            type = Random.Range(0, enemyTypes.Length);

            GameObject newEnemy = Instantiate(enemyTypes[type]);

            newEnemy.transform.position = position;

            enemies.Add(newEnemy);
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            enemyScripts.Add(enemyScript);
            enemyScript.Spawn();
            totalEnemiesSpawned++;
        }

        // Debug.Log("No enemy types available");
    }

    // stop tracking this enemy 
    public void LoseEnemy(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        enemyScripts.Remove(enemyScript);
        enemies.Remove(enemy);
    }

    public void InitEnemyManager()
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

        interval = initInterval;        // constant amount of time for the next wave

        initialized = true;
    }


    // spawn a group of zombies
    private IEnumerator SpawnGroup(int count, Vector2 position)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(position);
            yield return new WaitForSeconds(Random.Range(inWaveSpawnDelay * 0.25f, inWaveSpawnDelay * 1.75f));
        }

    }

    // generate a semi-random number that determines
    // how many zombies will spawn in the next wave
    private int RandomEnemyCount(float time)
    {
        // how to use the time parameter?
        // equation: f(x) = 0.05 * t^1.1 + 1
        // f(x) = Random.Range(f(x) * (1-waveSizeDeviation), f(x) * (1 + waveSizeDeviation))

        float initCount = 0.05f * Mathf.Pow(time, 1.1f) + 1;
        initCount = Random.Range(initCount * (1 - waveSizeDeviation), initCount * (1 + waveSizeDeviation));

        int finalCount = (int)initCount;

        Debug.Log($"Init count: {initCount}; final Count: {finalCount}");
        return finalCount;
    }


    // return a primary and secondary SpawnPosition
    private (Vector2, Vector2) PrimaryPosition()
    {
        // randomly choose between spawnPositions 0 and 1
        (Vector2, Vector2) tuple;

        // coinflip: true or false;
        bool pos1 = Random.value > 0.5f ? true : false;
        if (pos1)
        {
            tuple.Item1 = spawnPoints[0];
            tuple.Item2 = spawnPoints[1];
        }
        else
        {
            tuple.Item1 = spawnPoints[1];
            tuple.Item2 = spawnPoints[0];
        }
        Debug.Log($"Primary position: {tuple.Item1}; Secondary Position: {tuple.Item2}");
        return tuple;
    }

}
