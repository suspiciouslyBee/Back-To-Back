using UnityEngine;

public class TutorialPlayerManager : PlayerManager
{
    // Set variables
    private void Awake()
    {
        InitPlayerManager();
    }

    // Calls the individual players to swap and returns whether it was succesful for not
    override public bool Swap()
    {
        if (coolDownRemaining == 0 && (TutorialManager.TMInstance.tutorialStage == 3.5f || TutorialManager.TMInstance.tutorialStage == 3.75f))
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
                p1Swap = player1.Swap(solo);
            }
            if (player2 != null)
            {
                p2Swap = player2.Swap(solo);
            }
            StartCoroutine(knockback.Appear(0.1f));
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
            if (player1 != null && ((TutorialManager.TMInstance.tutorialStage > 1f && TutorialManager.TMInstance.tutorialStage < 2f) || TutorialManager.TMInstance.tutorialStage == 3.75f))
            {
                if (TutorialManager.TMInstance.tutorialStage == 1.5f)
                {
                    TutorialManager.TMInstance.nextStage();
                }
                return player1.UseWeapon(false).Item1;
            }
            return false;
        }
        (bool, bool) needAmmo = (false, false);
        if (player2 != null && (((TutorialManager.TMInstance.tutorialStage == 2.5f || TutorialManager.TMInstance.tutorialStage == 2.85f) && TutorialManager.TMInstance.tutorialStage < 3f) || TutorialManager.TMInstance.tutorialStage == 3.75f))
        {
            needAmmo = player2.UseWeapon(false);
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
    override public bool Reload(bool auto)
    {
        if ((TutorialManager.TMInstance.tutorialStage >= 2.75f && TutorialManager.TMInstance.tutorialStage < 3f) || TutorialManager.TMInstance.tutorialStage == 3.75f)
        {
            bool reloading = player2.Reload();
            if (TutorialManager.TMInstance.tutorialStage == 2.75f)
            {
                TutorialManager.TMInstance.nextStage();
            }
            return reloading;
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
}
