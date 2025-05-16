using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected bool startsActive;
    protected float lastCooldownStart;
    protected bool canUse;
    protected bool disabled;

    //------------------------- Getters -------------------------
    public float GetCoolDownTime()
    {
        /*
        Get the amount of time the ability is inactive between uses

        Inputs:
        * None

        Output:
        * Total cool down time
        */
        return cooldown;
    }
    public float GetTimeLeft()
    {
        /*
        Get the amount of time left before the ability can be sued again

        Inputs:
        * None

        Output:
        * Cool down time left
        */
        if (canUse)
        {
            return 0f;
        }
        return (cooldown - (lastCooldownStart - Time.time));
    }
    public bool GetDisabled()
    {
        /*
        Get whether  the ability is disabled or not

        Inputs:
        * None

        Output:
        * True: ability is disabled
          False: ability is not disabled
        */
        return disabled;
    }

    public bool GetCanUse()
    {
        /*
        Get whether the ability can be used or not

        Inputs:
        * None

        Output:
        * True: ability can be used
          False: ability is disabled or on cool down
        */
        if (GetDisabled())
        {
            return false;
        }
        return canUse;
    }

    //------------------------- Setters -------------------------
    public void DisableAbility()
    {
        /*
        Disable the ability until it is re-enabled

        Inputs:
        * None

        Output:
        * None
        */
        disabled = true;
    }

    public void EnableAbility()
    {
        /*
        Re-enable an ability

        Inputs:
        * None

        Output:
        * None
        */
        disabled = false;
    }

    //------------------------- Actions -------------------------
    public bool UseAbility()
    {
        /*
        Try to use the ability.

        Inputs:
        * None

        Output:
        * True: The ability is used
          False: The ability was on cooldown or disabled
        */
        if (GetCanUse())
        {
            StartCoroutine(AbilityCooldown());
            ActivateCustomEffect();
            return true;
        }
        return false;
    }

    // The custom effect the ability causes
    protected abstract void ActivateCustomEffect();

    private IEnumerator AbilityCooldown()
    {
        /*
        Turn off the ability until the cooldown finishes

        Inputs:
        * None

        Output:
        * None
        */
        canUse = false;
        lastCooldownStart = Time.time;
        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}
