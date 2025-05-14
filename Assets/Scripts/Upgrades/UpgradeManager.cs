using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private float levelIncreaseMult;           // The amount of exp each level increases by (1.0 = no change)
    [SerializeField] private int firstLevelUpExp;               // The amount of exp to achive the first level up
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;


    //------------------------- Getters -------------------------

    public int GetExpForLevel(int level)
    {
        /*
        Return a value for the exp needed to level up based on the level passed in (Example: If the level passed in is 0, finds the exp to move to level 1)

        Inputs:
        * The current level

        Output:
        * Exp needed for next level
        */
        return (int)(firstLevelUpExp * Mathf.Pow(levelIncreaseMult, level));
    }

    //------------------------- Setters -------------------------

    public void AddExpFromPlayer(int exp, GameObject killingPlayer)
    {
        Debug.Log("Adding EXP");
        /*
        Given the exp from an enemy dying and the player who killed it, add the exp to the other player and handle any level ups

        Inputs:
        * exp: The amount of exp to add
        * killingPlayer: The player that killed the enemy

        Output:
        * None
        */

        // Find the player opposite of the killing player to give exp to
        UpgradePathTracker upgradePath;
        GameObject curPlayer;
        // Player 1 killed an enemy
        if(killingPlayer == Player1)
        {
            curPlayer = Player2;
            
        }
        // Player 2 killed an enemy
        else
        {
            curPlayer = Player1;
        }

        // Add the exp
        upgradePath = curPlayer.GetComponent<UpgradePathTracker>();
        bool didLevelUp = upgradePath.AddExp(exp);

        // if a level up occured, have that player pull a new weapon
        if(didLevelUp)
        {
            GameObject newWeapon = upgradePath.GetCurWeapon();
            curPlayer.GetComponent<Player>().SwitchWeapon(newWeapon);
        }

    }

    //------------------------- Actions -------------------------



}
