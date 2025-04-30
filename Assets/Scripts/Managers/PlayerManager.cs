using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player player1;                // Player 1 aka the melee player
    [SerializeField] Player player2;                // Player 2 aka the ranged player
    float swapCoolDown;                             // How long of a cooldown do the players have, modifyable if we want to have upgrades n such
    bool swapped;                                   // Does the player have to wait to swap again

    // Set variables
    private void Start()
    {
        swapCoolDown = 5.0f;
        swapped = false;
    }

    // Calls the individual players to swap and returns whether it was succesful for not
    public bool Swap()
    {
        if (!swapped)
        {
            StartCoroutine(SwapTimer());
            return (player1.Swap() && player2.Swap());
        }
        return false;
    }
    
    // Call the player's attack function
    public bool Attack(bool Left)
    {
        if (Left && player1.left || !Left && !player1.left)  // If the input is for left attack and player1 is on the left side or the-
        {                                                   // input is right and player1 is on the right side, have player 1 attack
            return player1.UseWeapon();
        }
        return player2.UseWeapon();

        // Or can be changed to this if we want the inputs to be melee/range instead of left/right
        /* 
        if (melee) 
        {
            return player1.UseWeapon();
        }
        return player2.UseWeapon(); 
         */
    }

    // Because player2 will be the ranged individual, simply call player2 to always reload
    public bool Reload()
    {
        return player2.Reload();
    }

    // A short timer to limit how quickly you can swap
    IEnumerator SwapTimer()
    {
        swapped = true;
        yield return new WaitForSeconds(swapCoolDown);
        swapped = false;
    }
}
