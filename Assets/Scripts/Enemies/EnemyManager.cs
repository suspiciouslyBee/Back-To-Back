using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

// Singleton pattern
public class EnemyManager : MonoBehaviour
{

    // public GameManager gameManager
    protected static EnemyManager _instance;

    public GameObject[] enemyTypes;                                     // holds Prefabs of different enemies

    public float[] enemyWeights;                                        // correspond to indices in enemyTypes. Higher weights are less common.

    public float[] minWave;                                             // the earliest wave that this enemy type can spawn in

    public static EnemyManager Instance { get { return _instance; } }

    public List<GameObject> enemies;
    public List<Enemy> enemyScripts;

    public List<Vector2> spawnPoints;

    [SerializeField] private readonly float initInterval = 10f;           // initial interval between waves



    [SerializeField] private readonly float intervalDeviation = 0.5f;   // how much variance between waves there can be.

    [SerializeField] private readonly int initWaveWeight = 10;          // the total weight of the first zombie wave

    [SerializeField] private readonly float waveWeightIncrease = 1.5f;  // this means that waveWeight

    [SerializeField] protected readonly float waveSizeDeviation = 0.25f;     // how much the size of each wave should deviate

    public int wavesSpawned = 0;                                   // number of waves spawned

    private float waveWeight;

    private float timeWithNoEnemies;
    private float timeSinceLastWave;

    protected const float fixedUpdateTime = 1 / 60f;

    public bool initialized = false;

    [SerializeField] private float inWaveSpawnDelay = 0.05f;     // when spawning a group of zombies, produce a slight delay between
                                                                 // spawns

    [SerializeField] private float waveSpawnTimeMult = 1f;      // controls how rapidly zombie waves spawn

    [SerializeField] private const int spawnCap = 50;           // maximum number of zombies allowed on screen

    private readonly float maxWaveDelay = 30f;                  // longest the player should wait between waves

    public int totalEnemiesSpawned = 0;

    private AudioSource audioSource;


    // private wave-related variables
    protected float interval;


    [SerializeField] private List<GameObject> waveQueue;                     // all enemies in the wave
    [SerializeField] private List<GameObject> primaryQueue;                 // all enemies spawning at the primary position
    [SerializeField] private List<GameObject> secondaryQueue;               // all enemies spawning at the secondary position

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
        timeSinceLastWave += Time.deltaTime;
        if (GetEnemyCount() <= 0)
        {
            timeWithNoEnemies += Time.deltaTime;
        }
        else
        {
            timeWithNoEnemies = 0f;
        }

        if (timeSinceLastWave >= maxWaveDelay || timeWithNoEnemies > interval)
        {
            timeSinceLastWave = 0f;
            GenerateWave();
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
    // ###### WAVE LOGIC FUNCTIONS ######
    // function that masterminds a wave
    private void GenerateWave()
    {
        (Vector2, Vector2) positions = PrimaryPosition();
        Vector2 primaryPos = positions.Item1;
        Vector2 secondaryPos = positions.Item2;
        // populate the waves
        float actualWeight = Mathf.Floor(Random.Range(waveWeight * (1 - waveSizeDeviation), waveWeight * (1 + waveSizeDeviation)));
        float totalWeight = 0;

        while (totalWeight < actualWeight)
        {
            // choose a random enemy from EnemyTypes
            int tentativeIndex = Random.Range(0, enemyTypes.Length);

            if (wavesSpawned >= minWave[tentativeIndex])
            {
                GameObject tentativeEnemy = enemyTypes[tentativeIndex];
                waveQueue.Add(tentativeEnemy);
                totalWeight += enemyWeights[tentativeIndex];
            }
        }
        // empty the queues
        StartCoroutine(SpawnAllInList(primaryQueue, primaryPos));
        StartCoroutine(SpawnAllInList(secondaryQueue, secondaryPos));

        // make the next wave harder
        waveWeight *= 1.5f;
    }

    // spawns all enemies in the list in order and clears the list
    private IEnumerator SpawnAllInList(List<GameObject> enemies, Vector2 position)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            yield return new WaitForSeconds(inWaveSpawnDelay * Random.Range(0.75f, 1.25f));
            InitNewEnemy(enemies[i], position);
        }

        enemies.Clear();
    }

    // ###### HELPER FUNCTIONS ######

    virtual public int GetEnemyCount()
    {
        return enemies.Count;
    }

    private void InitNewEnemy(GameObject enemy, Vector2 position)
    {
        enemy.transform.position = position;

        enemies.Add(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScripts.Add(enemyScript);
        enemyScript.Spawn();
        totalEnemiesSpawned++;
    }

    // stop tracking this enemy
    public void LoseEnemy(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        enemyScripts.Remove(enemyScript);
        enemies.Remove(enemy);
    }

    virtual public void InitEnemyManager()
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
        timeWithNoEnemies = 0f;
        timeSinceLastWave = 0f;

        waveWeight = initWaveWeight;

        interval = initInterval;        // constant amount of time for the next wave

        initialized = true;
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
        //Debug.Log($"Primary position: {tuple.Item1}; Secondary Position: {tuple.Item2}");
        return tuple;
    }
    virtual public void AdjustSpawning(float newTiming)
    {
        interval = newTiming;
    }
}
