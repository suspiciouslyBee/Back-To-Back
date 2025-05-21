using UnityEngine;

public class TutorialPlayerManager : PlayerManager
{
    [SerializeField] Ability player1Prefab;
    [SerializeField] Ability player2Prefab;

    // Set variables
    private void Awake()
    {
        InitPlayerManager();
    }

    // Calls the individual players to swap and returns whether it was succesful for not
    override public bool Swap()
    {
        if (coolDownRemaining == 0 && (TutorialManager.TMInstance.tutorialStage == 3.5f || TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f))
        {
            if (TutorialManager.TMInstance.tutorialStage == 3.5f)
            {
                TutorialManager.TMInstance.tutorialStage = 3.65f;
                TutorialManager.TMInstance.nextStage();
            }
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
                player1.SetSprite(true);
                p1Swap = player1.Swap(solo);
            }
            if (player2 != null)
            {
                p2Swap = player2.Swap(solo);
            }
            StartCoroutine(knockback.Appear(0.1f));
/*            if (!InputPreference.meleeRanged)
            {
                HUDManager.Instance.ChangeBars(5, false);
            }*/
            return (p1Swap && p2Swap);
        }
        return false;
    }

    // Call the player's attack function
    override public bool Attack(bool melee)
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
            if (player1 != null && ((TutorialManager.TMInstance.tutorialStage > 1f && TutorialManager.TMInstance.tutorialStage < 2f) || TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f))
            {
                if (TutorialManager.TMInstance.tutorialStage == 1.5f)
                {
                    TutorialManager.TMInstance.nextStage();
                }
                return player1.UseWeapon().Item1;
            }
            return false;
        }
        (bool, bool) needAmmo = (false, false);
        if (player2 != null && (((TutorialManager.TMInstance.tutorialStage == 2.5f || TutorialManager.TMInstance.tutorialStage == 2.85f) && TutorialManager.TMInstance.tutorialStage < 3f) || TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f))
        {
            needAmmo = player2.UseWeapon();
            if (needAmmo.Item1 && !needAmmo.Item2)
            {
                if (TutorialManager.TMInstance.tutorialStage == 2.5f)
                {
                    TutorialManager.TMInstance.tutorialStage = 2.55f;
                    TutorialManager.TMInstance.nextStage();
                }
                else
                {
                    Reload(true);
                }
            }
        }
        return needAmmo.Item1;
    }

    // Because player2 will be the ranged individual, simply call player2 to always reload
    override public bool Reload(bool auto = false)
    {
        if (!player2.acting && ((TutorialManager.TMInstance.tutorialStage >= 2.75f && TutorialManager.TMInstance.tutorialStage < 3f) || TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f))
        {
            bool reloading = player2.Reload();
            if (TutorialManager.TMInstance.tutorialStage == 2.75f)
            {
                TutorialManager.TMInstance.nextStage();
            }
            if (!auto)
            {
                player2.SetSprite(reloading);
            }
            return reloading;
        }
        else if ((TutorialManager.TMInstance.tutorialStage >= 2.75f && TutorialManager.TMInstance.tutorialStage < 3f) || TutorialManager.TMInstance.tutorialStage == 3.75f || TutorialManager.TMInstance.tutorialStage == 4.5f)
        {
            HUDManager.Instance.ChangeBars(2.5f, true);
            return false;
        }
        return false;
    }

    // Calls the abilities of the chosen player
    override public bool CallAbility(bool player, bool ability)
    {
        if (TutorialManager.TMInstance.tutorialStage == 4.5f)
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
        return false;
    }

    // naive way of counting number of living players
    override public void UpdatePlayerCount()
    {
        playerCount = 0;
        TutorialManager.TMInstance.tutorialPause = true;
        if (player1 != null)
        {
            player1.Hurt(100);
        }
        if (player2 != null)
        {
            player2.Hurt(100);
        }
        // if (player1 != null) playerCount++;
        // if (player2 != null) playerCount++;
    }

    // Initalizes the static player manager
    override public void InitPlayerManager()
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

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public override void GiftPlayers(bool expOrAbilities)
    {
        if (expOrAbilities)
        {
            UpgradeManager uM = this.gameObject.GetComponent<UpgradeManager>();
            uM.AddExpFromPlayer(uM.GetExpForLevel(0), player1.gameObject);
            uM.AddExpFromPlayer(uM.GetExpForLevel(0), player2.gameObject);
        }
        else
        {
            player1.SetAbility(player1Prefab, true);
            player2.SetAbility(player2Prefab, true);
            HUDManager.Instance.ChangeBars(6, false);
        }
    }
}
