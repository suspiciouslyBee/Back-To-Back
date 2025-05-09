using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private float levelIncreaseMult;           // The amount of exp each level increases by (1.0 = no change)
    [SerializeField] private int firstLevelUpExp;               // The amount of exp to achive the first level up


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



    //------------------------- Actions -------------------------



}
