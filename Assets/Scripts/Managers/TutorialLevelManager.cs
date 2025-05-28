using System;
using UnityEngine;

public class TutorialLevelManager : LevelManager
{
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

    public override void FireGun()
    {
        PCInstance.Attack(false);
    }

    public override void ReloadGun()
    {
        PCInstance.Reload();
    }

    public override void RangedAbility1()
    {
        PCInstance.CallAbility(false, true);
    }

    public override void RangedAbility2()
    {
        PCInstance.CallAbility(false, false);
    }

    public override void SwingSword()
    {
        PCInstance.Attack(true);
    }

    public override void MeleeAbility1()
    {
        PCInstance.CallAbility(true, true);
    }

    public override void MeleeAbility2()
    {
        PCInstance.CallAbility(true, false);
    }

    public override void SwapChars()
    {
        if (!gameOver)
        {
            PCInstance.Swap();
        }
        else
        {
            PauseManager.Instance.Select();
        }
    }

    public override void Pause()
    {
        
    }

    public override void Up()
    {
        if (gameOver)
        {
            PauseManager.Instance.Movement(true);
        }
    }

    public override void Down()
    {
        if (gameOver)
        {
            PauseManager.Instance.Movement(false);
        }
    }
}
