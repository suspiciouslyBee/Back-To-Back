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
using UnityEngine.PlayerLoop;

public class LevelManager : MonoBehaviour
{
    public static MainManager Instance;

    //Makes into local/scene-specific singleton
    public static LevelManager LMInstance;

    public PlayerManager PCInstance;

    public UIManager UIInstance;

    public bool gameOver;                       // if true, the game is over!

    public float timeSurvived;

    [SerializeField] private EnemyManager enemyManager;

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


        timeSurvived = 0f;
        gameOver = false;
    }

    void Start()
    {
        CheckForLevelLogic();
    }

    bool CheckForLevelLogic()
    {
        //if the EM is not specified we will attempt to fill it
        if (enemyManager == null)
        {
            // Debug.Log("Enemy Manager not explictly specified for this level!\nSearching...");
            enemyManager = EnemyManager.Instance;
        }

        //checks again and throws an exception if we still have nothing
        if (enemyManager == null)
        {
            throw new NullReferenceException("Can't find Enemy Manager for Scene!");
        }

        if (UIManager.Instance == null)
        {

            throw new NullReferenceException("Can't find UI Manager for Scene!");
        }

        return true;
    }


    protected virtual void FixedUpdate()
    {
        if (!gameOver && PCInstance.playerCount == 0)
        {
            // TODO: trigger game over
            UIInstance.GameOverSequence();
            gameOver = true;
        }
        else if (!gameOver)
        {
            timeSurvived += Time.deltaTime;
            HUDManager.Instance.ChangeBars(4, false);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            timeSurvived += 30f;
        }
    }

    //running on assumptions
    //can be overridden by a class that inherits LM?

    public virtual void FireGun()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.Attack(false);
        }
    }

    public virtual void ReloadGun()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.Reload();
        }
    }

    public virtual void RangedAbility1()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.CallAbility(false, true);
        }
    }

    public virtual void RangedAbility2()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.CallAbility(false, false);
        }
    }

    public virtual void SwingSword()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.Attack(true);
        }
    }

    public virtual void MeleeAbility1()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.CallAbility(true, true);
        }
    }

    public virtual void MeleeAbility2()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.CallAbility(true, false);
        }
    }

    public virtual void SwapChars()
    {
        if (!PauseManager.Instance.isPaused)
        {
            PCInstance.Swap();
        }
        else
        {
            PauseManager.Instance.Select();
        }
    }

    public virtual void Pause()
    {
        PauseManager.Instance.PauseGame();
    }

    public virtual void Up()
    {
        if (gameOver)
        {

        }
        else if (PauseManager.Instance.isPaused)
        {
            PauseManager.Instance.Movement(true);
        }
    }

    public virtual void Down()
    {
        if (gameOver)
        {

        }
        else if (PauseManager.Instance.isPaused)
        {
            PauseManager.Instance.Movement(false);
        }
    }

    // initialize everything that depends on this
    private void InitDependents()
    {
        UIManager.Instance.InitUIManager();
        HUDManager.Instance.InitHUDManager();
        PCInstance.InitPlayerManager();
        EnemyManager.Instance.InitEnemyManager();
    }

}
