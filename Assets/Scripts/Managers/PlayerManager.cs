using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PlayerManager : MonoBehaviour
{
    protected static PlayerManager PMInstance;

    public static PlayerManager Instance { get { return PMInstance; } }
    public bool initialized = false;
    [SerializeField] protected Player player1;                          // Player 1 aka the melee player
    [SerializeField] protected Player player2;                          // Player 2 aka the ranged player
    [SerializeField] protected SwapKnockBack knockback;                 // Push back collider for swapping

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip swapSFX;
    protected float swapCoolDown;                                       // How long of a cooldown do the players have, modifyable if we want to have upgrades n such
    protected float coolDownRemaining;                                  // Stores current cool down time for swapping

    public int playerCount;
    protected bool swappedDirection;

    // Set variables
    private void Awake()
    {
        InitPlayerManager();
    }

    // Function for healing both players
    public void HealPlayers(float healAmount1, float healAmount2)
    {
        if (player1 != null)
        {
            player1.Heal(healAmount1);
        }
        if (player2 != null)
        {
            player2.Heal(healAmount2);
        }
        HUDManager.Instance.ChangeBars(1, false);
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
                HUDManager.Instance.ChangeBars(7f, false);
            }
            else
            {
                solo = true;
            }
            StartCoroutine(SwapTimer());
            swappedDirection = !swappedDirection;
            bool p1Swap = false;
            bool p2Swap = false;
            if (player1 != null)
            {
                p1Swap = player1.Swap(solo);
                player1.SetSprite(true);
            }
            if (player2 != null)
            {
                p2Swap = player2.Swap(solo);
            }
            StartCoroutine(knockback.Appear(0.1f));
            AudioManager.Instance.PlayAudio(swapSFX, 0.07f);
            if (!InputPreference.meleeRanged)
            {
                HUDManager.Instance.ChangeBars(5, false);
            }
            return (p1Swap && p2Swap);
        }
        HUDManager.Instance.ChangeBars(3.5f, true);
        return false;
    }

    // Call the player's attack function
    virtual public bool Attack(bool melee)
    {
        if (InputPreference.meleeRanged)
        {
            // Melee or ranged attacks
            if (melee)
            {
                if (player1 != null)
                {
                    return player1.UseWeapon().Item1;
                }
                return false;
            }
            (bool, bool) needAmmo = (false, false);
            if (player2 != null)
            {
                needAmmo = player2.UseWeapon();
                if (needAmmo.Item1 && !needAmmo.Item2)
                {
                    Reload(true);
                }
            }
            return needAmmo.Item1;
        }
        else
        {
            // Melee is filling the role of LEFT in this context
            if (melee && player1.left || !melee && !player1.left)  // If the input is for left attack and player1 is on the left side or the-
            {                                                      // input is right and player1 is on the right side, have player 1 attack
                if (player1 != null)
                {
                    return player1.UseWeapon().Item1;
                }
                return false;
            }
            (bool, bool) needAmmo = (false, false);
            if (player2 != null)
            {
                needAmmo = player2.UseWeapon();
                if (needAmmo.Item1 && !needAmmo.Item2)
                {
                    Reload(true);
                }
            }
            return needAmmo.Item1;
        }
    }

    // Because player2 will be the ranged individual, simply call player2 to always reload
    virtual public bool Reload(bool auto = false)
    {
        if (!player2.acting)
        {
            bool temp = player2.Reload();
            if (!auto)
            {
                player2.SetSprite(temp);
            }
            return temp;
        }
        else
        {
            HUDManager.Instance.ChangeBars(2.5f, true);
            return false;
        }
    }

    // Calls the abilities of the chosen player
    virtual public bool CallAbility(bool player, bool ability)
    {
        // Keep them the same
        if (InputPreference.meleeRanged || player1.left)
        {
            if (player)
            {
                if (player1 == null)
                {
                    return false;
                }
                if (ability)
                {
                    return player1.CallAbility(ability);
                }
                return player1.CallAbility(ability);
            }
            if (player2 == null)
            {
                return false;
            }
            if (ability)
            {
                return player2.CallAbility(ability);
            }
            return player2.CallAbility(ability);
        }

        if (player)
        {
            if (player2 == null)
            {
                return false;
            }
            if (ability)
            {
                return player2.CallAbility(ability);
            }
            return player2.CallAbility(ability);
        }
        if (player1 == null)
        {
            return false;
        }
        if (ability)
        {
            return player1.CallAbility(ability);
        }
        return player1.CallAbility(ability);
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

    // naive way of counting number of living players
    virtual public void UpdatePlayerCount()
    {
        playerCount--;
        if (playerCount == 1)
        {
            StartCoroutine(knockback.Appear(0.1f));
            swapCoolDown /= 2;
            if (player1.dead)
            {
                player2.transform.position = new Vector2(0, -1.8f);
                HUDManager.Instance.ChangeBars(7.5f, true);
            }
            else
            {
                player1.transform.position = new Vector2(0, -1.8f);
                HUDManager.Instance.ChangeBars(7.5f, false);
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

    public ((bool, bool), (bool, bool)) GetAbilityInfo()
    {
        ((bool, bool), (bool, bool)) results = ((false, false), (false, false));
        if (player1 != null)
        {
            results.Item1 = player1.GetAbilityInfo();
        }
        if (player2 != null)
        {
            results.Item2 = player2.GetAbilityInfo();
        }
        return results;
    }

    public bool GetDirectionInfo()
    {
        return swappedDirection;
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
            initialized = true;
        }
        swapCoolDown = 3.0f;
        coolDownRemaining = 0;
        playerCount = 2;
        swappedDirection = false;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Mainly for tutorial
    virtual public void GiftPlayers(bool expOrAbilities) {}
}
