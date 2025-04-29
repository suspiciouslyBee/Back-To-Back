using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    bool p1isLeft;                                // To keep track of which side the players are on
    float swapCoolDown;
    bool swapped;

    // Set variables
    private void Start()
    {
        swapCoolDown = 5.0f;
        p1isLeft = true;
        swapped = false;
    }

    // Calls the individual players to swap and returns whether it was succesful for not
    public bool Swap()
    {
        if (!swapped)
        {
            StartCoroutine(SwapTimer());
            p1isLeft ^= p1isLeft;
            return (player1.Swap() && player2.Swap());
        }
        return false;
    }
    
    // Call the player's attack function
    public bool Attack(bool Left)
    {
        if (Left && p1isLeft || !Left && !p1isLeft) // If the input is for left attack and player1 is on the left side or the input
        {                                           // is right and player1 is on the right side, have player 1 attack
            return player1.UseWeapon();
        }
        return player2.UseWeapon();
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
