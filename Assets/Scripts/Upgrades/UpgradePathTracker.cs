using UnityEngine;

public class UpgradePathTracker : MonoBehaviour
{
    [SerializeField] private GameObject progressionTree;        // The outer array tracks level, inner array tracks melee weapons available at that level
    private GameObject weapon;                                  // The current weapon of the player
    private UpgradeManager UpgradeScript;                       // The script for the upgrade manager
    private int level;                                          // The current Level of the player
    private int exp;                                            // The current EXP of the player
    private int nextLevelExp;                                   // The exp needed to level up. Dictated by upgrade manager
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 0;
        exp = 0;
        UpgradeScript = transform.parent.gameObject.GetComponent<UpgradeManager>();
        SetNextLevelExp();
    }

    //------------------------- Getters -------------------------

    public int GetCurrentLevel()
    {
        /*
        Get the current Level of the player

        Inputs:
        * None

        Output:
        * The currenet level of the player
        */
        return level;
    }

    public int GetCurrentExp()
    {
        /*
        Get the current exp of the player

        Inputs:
        * None

        Output:
        * The current exp of the player
        */
        return exp;
    }

    public int GetLevelUpExp()
    {
        /*
        The total exp needed to reach the next level

        Inputs:
        * None

        Output:
        * The total exp for next level
        */
        return nextLevelExp;
    }

    public GameObject GetCurWeapon()
    {
        /*
        The current weapon selected for the player

        Inputs:
        * None

        Output:
        * The current weapon selected for the player
        */
        if (weapon == null)
        {
            RandomSelectNextWeapon();
        }
        return weapon;
    }

    //------------------------- Setters -------------------------

    public bool AddExp(int amount)
    {
        /*
        Add an amount of exp to the player

        Inputs:
        * The amount of exp to add

        Output:
        * True: Player leveled up
        * False: Player did not level up
        */

        // Add the exp to the player
        exp += amount;
        bool leveledUp = false;

        // Check for level up
        while(exp >= nextLevelExp)
        {
            leveledUp = true;
            DoLevelUp();
        }

        // Return true if level up occured
        return leveledUp;
    }

    private void DoLevelUp()
    {
        /*
        Increase the players level, update the exp for the next level, and select a new weapon

        Inputs:
        * None

        Output:
        * None
        */

        level++;
        exp -= nextLevelExp;
        SetNextLevelExp();
        RandomSelectNextWeapon();
    }

    private void SetNextLevelExp()
    {
        /*
        Update the exp needed for the next level to the value for the current level

        Inputs:
        * None

        Output:
        * None
        */
        nextLevelExp = UpgradeScript.GetExpForLevel(level);
    }

    private void RandomSelectNextWeapon()
    {
        /*
        Select a new weapon for the player at random from the weapons listed in it's current level array

        Inputs:
        * None

        Output:
        * None
        */

        // Assign a random weapon
        Progression progressionArrays = progressionTree.GetComponent<Progression>();
        int numberOfWeapons = progressionArrays.NumberOfWeaponsForLevel(level); // Number of possible options
        int selectedindex = Random.Range(0, numberOfWeapons);                   // index of random weapon
        weapon = progressionArrays.GetWeaponFromLevel(selectedindex, level);    // Set the weapon
    }

    //------------------------- Actions -------------------------

}
