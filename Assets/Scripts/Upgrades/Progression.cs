using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] private GameObject[] level0;
    [SerializeField] private GameObject[] level1;
    [SerializeField] private GameObject[] level2;
    [SerializeField] private GameObject[] level3;
    [SerializeField] private GameObject[] level4;
    [SerializeField] private GameObject[] level5;
    private const int numberOfLevels = 6;

    //------------------------- Getters -------------------------

    public int NumLevels()
    {
        /*
        Get the number of levels supported

        Inputs:
        * None

        Output:
        * The number of levels supported
        */
        return numberOfLevels;
    }

    public int NumberOfWeaponsForLevel(int level)
    {
        /*
        The number of weapons on that level

        Inputs:
        * level: the level that the number of weapons to choose from

        Output:
        * The number of weapons on that level
        */

        // Fix the minimum level
        if (level < 0 )
        {
            level = 0;
        }

        // Return the length of the weapon array for the reque3sted level
        GameObject[] weaponArray;
        switch (level)
        {
            case 0:
                weaponArray = level0;
                break;
            case 1:
                weaponArray = level1;
                break;
            case 2:
                weaponArray = level2;
                break;
            case 3:
                weaponArray = level3;
                break;
            case 4:
                weaponArray = level4;
                break;
            default:
                weaponArray = level5;
                break;
        }
        return weaponArray.Length;
    }

    public GameObject GetWeaponFromLevel(int index, int level)
    {
        /*
        Get a weapon from the selected index

        Inputs:
        * index: the weapon within the level
        * level: the level to choose the index from

        Output:
        * A prefab weapon
        */

        // Fix the minimum level
        if (level < 0 )
        {
            level = 0;
        }

        // Return the length of the weapon array for the reque3sted level
        GameObject[] weaponArray;
        switch (level)
        {
            case 0:
                weaponArray = level0;
                break;
            case 1:
                weaponArray = level1;
                break;
            case 2:
                weaponArray = level2;
                break;
            case 3:
                weaponArray = level3;
                break;
            case 4:
                weaponArray = level4;
                break;
            default:
                weaponArray = level5;
                break;
        }
        
        // Fix the minimum index
        if(index >= weaponArray.Length)
        {
            index = weaponArray.Length - 1;
        }
        else if (index < 0)
        {
            index = 0;
        }

        // return the weapon game object
        return weaponArray[index];
    }
}

