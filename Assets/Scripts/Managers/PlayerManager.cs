using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager PMInstance;

    public static PlayerManager Instance { get { return PMInstance; } }
    public bool initialized = false;
    Player player1;                       // Player 1 aka the melee player
    Player player2;                       // Player 2 aka the ranged player
    float swapCoolDown;                   // How long of a cooldown do the players have, modifyable if we want to have upgrades n such
    float coolDownRemaining;              // Stores cool down time remaining

    public int playerCount;

    // Set variables
    private void Awake()
    {
        InitPlayerManager();
    }

    // Calls the individual players to swap and returns whether it was succesful for not
    public bool Swap()
    {
        if (coolDownRemaining == 0)
        {
            StartCoroutine(SwapTimer());
            return (player1.Swap() && player2.Swap());
        }
        return false;
    }

    // Call the player's attack function
    public bool Attack(bool melee)
    {
        /*
        if (Left && player1.left || !Left && !player1.left)  // If the input is for left attack and player1 is on the left side or the-
        {                                                   // input is right and player1 is on the right side, have player 1 attack
            return player1.UseWeapon();
        }
        return player2.UseWeapon();
        */

        // Or can be changed to this if we want the inputs to be melee/range instead of left/right
        if (melee) 
        {
            return player1.UseWeapon();
        }
        bool needAmmo = player2.UseWeapon();
        return needAmmo; 
    }

    // Because player2 will be the ranged individual, simply call player2 to always reload
    public bool Reload()
    {
        return player2.Reload();
    }

    // A short timer to limit how quickly you can swap
    IEnumerator SwapTimer()
    {
        coolDownRemaining = swapCoolDown;
        while (coolDownRemaining > 0)
        {
            HUDManager.Instance.changeBars(3, false);
            coolDownRemaining -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        coolDownRemaining = 0;
        HUDManager.Instance.changeBars(3, false);
    }

    // naive way of counting number of living players
    public void UpdatePlayerCount()
    {
        playerCount--;
        // if (player1 != null) playerCount++;
        // if (player2 != null) playerCount++;
    }

    // Return the max amount of cooldown and the time remaining on that cooldown
    public (float, float) GetSwapInfo()
    {
        return (swapCoolDown, coolDownRemaining);
    }

    // Return the max amount of health and cur health for each player
    public ((float, float), (float, float)) GetHealthInfo()
    {
        ((float, float), (float, float)) info;
        if (player1 != null)
        {
            info.Item1 = player1.GetHealthInfo();
        }
        else
        {
            info.Item1 = (1, 0);
        }
        if (player2 != null)
        {
            info.Item2 = player2.GetHealthInfo();
        }
        else
        {
            info.Item2 = (1, 0);
        }
        return info;
    }

    // Gets the gun player's max ammo and cur ammo 
    public (float, float) GetAmmoInfo()
    {
        if (player2 != null)
        {
            return player2.GetAmmoInfo();
        }
        else
        {
            return (1, 0);
        }
    }

    // Initalizes the static player manager
    public void InitPlayerManager()
    {
        if (PMInstance != null && PMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PMInstance = this;
        }
        swapCoolDown = 3.0f;
        coolDownRemaining = 0;
        playerCount = 2;
        initialized = true;
        player1 = transform.Find("Player1").GetComponent<Player>();
        player2 = transform.Find("Player2").GetComponent<Player>();
    }
}
