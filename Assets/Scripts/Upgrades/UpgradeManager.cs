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
        // Assign exp to the opposite player of the player that killed
        // If that player is dead, assing the exp to the killing player
        // If both players are dead, do nothing
        bool player1EXPEvent = (Player1 != null) && ((killingPlayer == Player2) || (Player2 == null));
        bool player2EXPEvent = (Player2 != null) && ((killingPlayer == Player1) || (Player1 == null));

        if (player1EXPEvent)
        {
            curPlayer = Player1;

        }
        else if (player2EXPEvent)
        {
            curPlayer = Player2;
        }
        else
        {
            return;
        }

        // Add the exp
        upgradePath = curPlayer.GetComponent<UpgradePathTracker>();
        bool didLevelUp = upgradePath.AddExp(exp);

        // if a level up occured, have that player pull a new weapon
        if (didLevelUp)
        {
            GameObject newWeapon = upgradePath.GetCurWeapon();
            curPlayer.GetComponent<Player>().SwitchWeapon(newWeapon);
        }

    }

    //------------------------- Actions -------------------------



}
