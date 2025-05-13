using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PlayerManager : MonoBehaviour
{
    protected static PlayerManager PMInstance;

    public static PlayerManager Instance { get { return PMInstance; } }
    public bool initialized = false;
    protected Player player1;                       // Player 1 aka the melee player
    protected Player player2;                       // Player 2 aka the ranged player
    [SerializeField] protected Collider2D knockback;// Push back collider for swapping

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip swapSFX;
    protected float swapCoolDown;                   // How long of a cooldown do the players have, modifyable if we want to have upgrades n such
    protected float coolDownRemaining;              // Stores cool down time remaining

    public int playerCount;

    // Set variables
    private void Awake()
    {
        InitPlayerManager();
    }
    // call tick functions for each player 
    public void PlayerTick()
    {
        if (player1 != null)
        {
            player1.Tick();
        }

        if (player2 != null)
        {
            player2.Tick();
        }
    }
    // Calls the individual players to swap and returns whether it was succesful for not
    virtual public bool Swap()
    {
        if (coolDownRemaining == 0)
        {
            bool solo;
            if (playerCount == 2)
            {
                solo = false;
            }
            else
            {
                solo = true;
            }
            StartCoroutine(SwapTimer());
            bool p1Swap = false;
            bool p2Swap = false;
            if (player1 != null)
            {
                p1Swap = player1.Swap(solo);
            }
            if (player2 != null)
            {
                p2Swap = player2.Swap(solo);
            }
            StartCoroutine(pushAway());
            audioSource.PlayOneShot(swapSFX);
            return (p1Swap && p2Swap);
        }
        return false;
    }

    // Call the player's attack function
    virtual public bool Attack(bool melee)
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
            if (player1 != null)
            {
                return player1.UseWeapon().Item1;
            }
            return false;
        }
        bool needAmmo = false;
        if (player2 != null)
        {
            needAmmo = player2.UseWeapon().Item1;
        }
        return needAmmo;
    }

    // Because player2 will be the ranged individual, simply call player2 to always reload
    virtual public bool Reload()
    {
        return player2.Reload();
    }

    // A short timer to limit how quickly you can swap
    protected IEnumerator SwapTimer()
    {
        coolDownRemaining = swapCoolDown;
        while (coolDownRemaining > 0)
        {
            HUDManager.Instance.ChangeBars(3, false);
            coolDownRemaining -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        coolDownRemaining = 0;
        HUDManager.Instance.ChangeBars(3, false);
    }

    protected IEnumerator pushAway()
    {
        knockback.enabled = true;
        yield return new WaitForSeconds(0.1f);
        knockback.enabled = false;
    }

    // naive way of counting number of living players
    virtual public void UpdatePlayerCount()
    {
        playerCount--;
        if (playerCount == 1)
        {
            StartCoroutine(pushAway());
            swapCoolDown /= 2;
            if (player1.dead)
            {
                player2.transform.position = new Vector2(0, -1.8f);
            }
            else
            {
                player1.transform.position = new Vector2(0, -1.8f);
            }
        }
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
    virtual public void InitPlayerManager()
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
        knockback.enabled = false;
        player1 = transform.Find("Player1").GetComponent<Player>();
        player2 = transform.Find("Player2").GetComponent<Player>();

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
}
