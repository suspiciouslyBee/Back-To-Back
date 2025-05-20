using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

/* 
    How to add new enemies to Enemy Manager
    1. assign a prefab to enemyTypes
    2. assign a weight in enemyWeights
    3. assign a minWave value

    weight displaces other zombies in the wave.
    minWave is the earliest that a zombie can spawn.
*/

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

    [SerializeField] protected float initInterval = 10f;           // initial interval between waves



    protected readonly float intervalDeviation = 0.5f;   // how much variance between waves there can be.

    [SerializeField] protected int initWaveWeight = 10;          // the total weight of the first zombie wave

    [SerializeField] protected float waveWeightIncrease = 1.5f;  // this means that waveWeight

    [SerializeField] protected float waveSizeDeviation = 0.25f;     // how much the size of each wave should deviate

    public int wavesSpawned = 0;                                   // number of waves spawned



    protected const float fixedUpdateTime = 1 / 60f;

    public bool initialized = false;

    [SerializeField] protected float inWaveSpawnDelay = 0.05f;     // when spawning a group of zombies, produce a slight delay between
                                                                   // spawns

    [SerializeField] protected float waveSpawnTimeMult = 1f;      // controls how rapidly zombie waves spawn

    [SerializeField] protected int spawnCap = 50;           // maximum number of zombies allowed on screen

    [SerializeField] protected float maxWaveDelay = 30f;                  // longest the player should wait between waves

    public int totalEnemiesSpawned = 0;

    protected AudioSource audioSource;


    // protected wave-related variables
    [SerializeField] protected float interval;                     // how long the game should wait with zero zombies on screen before spawning more
    [SerializeField] protected float minInterval;                   // the absolute minimum amount of break time the player gets


    [SerializeField] protected float waveWeight;                  // how "heavy" the wave should be

    [SerializeField] protected float timeWithNoEnemies;           // how long it's been with 0 enemies on screen
    [SerializeField] protected float timeSinceLastWave;           // how long since a wave was spawned

    [SerializeField] protected List<int> waveQueue;                     // all enemies in the wave
    [SerializeField] protected List<int> primaryQueue;                 // all enemies spawning at the primary position
    [SerializeField] protected List<int> secondaryQueue;               // all enemies spawning at the secondary position

    protected void Awake()
    {
        InitEnemyManager();
    }

    void Start()
    {
        Time.fixedDeltaTime = fixedUpdateTime;

    }

    // Remove Update as needed
    protected void Update()
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
            timeWithNoEnemies = 0f;
            GenerateWave();
            interval *= 0.9f;
            if (interval < minInterval)
            {
                interval = minInterval;
            }
        }



    }

    // tick 60 times per second
    protected void FixedUpdate()
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
    protected void GenerateWave()
    {
        wavesSpawned++;
        waveQueue.Clear();
        (Vector2, Vector2) positions = PrimaryPosition();
        Vector2 primaryPos = positions.Item1;
        Vector2 secondaryPos = positions.Item2;
        // populate the waves
        float actualWeight = Mathf.Floor(Random.Range(waveWeight * (1 - waveSizeDeviation), waveWeight * (1 + waveSizeDeviation)));
        float totalWeight = 0;

        for (int i = 0; i < spawnCap; i++)
        {
            // choose a random enemy from EnemyTypes
            int tentativeIndex = Random.Range(0, enemyTypes.Length);

            if (wavesSpawned >= minWave[tentativeIndex])
            {

                waveQueue.Add(tentativeIndex);
                totalWeight += enemyWeights[tentativeIndex];
                Debug.Log($"Adding enemy of index {tentativeIndex} to waveQueue");
            }
            if (totalWeight > actualWeight)
            {
                break;
            }
        }

        foreach (int i in waveQueue)
        {
            if (Random.Range(0f, 1.0f) > 0.8f)
            {
                primaryQueue.Add(i);
            }
            else
            {
                secondaryQueue.Add(i);
            }
        }


        // empty the queues
        StartCoroutine(SpawnAllInList(primaryQueue, primaryPos));
        StartCoroutine(SpawnAllInList(secondaryQueue, secondaryPos));

        // make the next wave harder
        waveWeight *= waveWeightIncrease;
    }

    // spawns all enemies in the list in order and clears the list
    protected IEnumerator SpawnAllInList(List<int> enemies, Vector2 position)
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

    protected void InitNewEnemy(int index, Vector2 position)
    {
        GameObject enemy = Instantiate(enemyTypes[index]);
        enemy.transform.position = position;

        enemies.Add(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetDestination(new());
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
    protected (Vector2, Vector2) PrimaryPosition()
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

    // ###### STUFF THAT TUTORIAL ENEMYMANAGER NEEDS TO INHERIT FOR SOME REASON ######
    // spawn an enemy somewhere idk
    virtual public void SpawnEnemy()
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


    // spawn a specific enemy from an index
    virtual public void SpawnEnemyType(int index, Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyTypes[index]);

        newEnemy.transform.position = position;

        enemies.Add(newEnemy);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScripts.Add(enemyScript);
        enemyScript.Spawn();
    }

    // remove all enemies from the scene. Use with caution.
    public void RemoveAllEnemies()
    {
        foreach (GameObject e in enemies)
        {
            if (e != null)
            {
                Destroy(e);
            }
        }
    }

}
