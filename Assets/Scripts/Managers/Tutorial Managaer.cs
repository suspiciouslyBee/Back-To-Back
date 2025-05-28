using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{
    private UIDocument Dialogue;

    public static TutorialManager TMInstance;
    [SerializeField] private EnemyManager enemyManager;

    InputAction progress;
    VisualElement clicker;
    Label dialogue;
    Label reminder;

    public bool tutorialPause;
    public float tutorialStage;
    bool idle;

    //The extra checks are here incase there is a duplicate by any means
    private void Awake()
    {
        if (TMInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        TMInstance = this;
        idle = false;

        tutorialStage = 1;
        tutorialPause = false;
    }

    private void Start()
    {
        //if the EM is not specified we will attempt to fill it
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.Instance;
            progress = InputSystem.actions.FindAction("SwapCharacters");
            Dialogue = gameObject.GetComponent<UIDocument>();
            dialogue = Dialogue.rootVisualElement.Q<Label>("Dialogue");
            reminder = Dialogue.rootVisualElement.Q<Label>("Reminder");
            clicker = Dialogue.rootVisualElement.Q<VisualElement>("Clicker");
            enemyManager.spawnPoints = new List<Vector2>();
            enemyManager.spawnPoints.Add(new Vector2(-10.3f, -1.8f));
            enemyManager.AdjustSpawning(0);
            StartCoroutine(dialogue1());
        }

        //checks again and throws an exception if we still have nothing
        if (enemyManager == null)
        {
            throw new NullReferenceException("Can't find Enemy Manager for Scene!");
        }
    }

    public void nextStage()
    {
        switch (tutorialStage)
        {
            case 1.5f:
                tutorialStage = 1.75f;
                tutorialPause = false;
                break;
            case 1.75f:
                tutorialPause = true;
                tutorialStage = 2;
                enemyManager.spawnPoints = new List<Vector2>();
                enemyManager.spawnPoints.Add(new Vector2(10.3f, -1.8f));
                StartCoroutine(dialogue2());
                break;
            case 2.5f:
                tutorialPause = false;
                StartCoroutine(spawn());
                break;
            case 2.55f:
                tutorialPause = true;
                StartCoroutine(dialogue3());
                break;
            case 2.75f:
                if (EnemyManager.Instance.GetEnemyCount() == 0)
                {
                    StartCoroutine(spawn());
                }
                StartCoroutine(dialogue4());
                break;
            case 2.85f:
                tutorialStage = 3;
                enemyManager.spawnPoints = new List<Vector2>();
                enemyManager.spawnPoints.Add(new Vector2(-10.3f, -1.8f));
                StartCoroutine(dialogue5());
                break;
            case 3.65f:
                StartCoroutine(dialogue6());
                break;
            case 3.75f:
                tutorialStage = 4;
                tutorialPause = true;
                StartCoroutine(dialogue7());
                break;
            case 4.5f:
                tutorialStage = 5;
                tutorialPause = true;
                StartCoroutine(dialogue8());
                break;
        }
    }

    IEnumerator dialogue1()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "O-Oh It’s you two?! I didn’t realize your post was still…nevermind.";
        for (int i = 0; i <= 8; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i) 
            {
                case 0:
                    dialogue.text = "Looking for some guidance huh?";
                    break;
                case 1:
                    dialogue.text = "Well luckily, I’ve become quite the zombie killing pro over the years. So with some time and training I’m sure you’ll be-";
                    break;
                case 2:
                    dialogue.text = "";
                    DeactivateClicker();
                    enemyManager.AdjustSpawning(0.1f);
                    yield return new WaitForSeconds(0.11f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(2.75f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "Have the other posts fallen already?!";
                    break;
                case 3:
                    dialogue.text = "Such a shame, oh well. I’ll just have to explain things, quicker.";
                    break;
                case 4:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "You see those two mercenaries at the center of the screen? That's where you are located.";
                    break;
                case 5:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "The person on the left uses melee weapons, and the person of the right uses ranged weapons.";
                    break;
                case 6:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "At the top of the screen are your health bars.";
                    break;
                case 7:
                    DeactivateClicker();
                    dialogue.text = ".";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    dialogue.text = ". .";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    dialogue.text = ". . .";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "That zombie's getting way too close, go ahead and deal with it.";
                    break;
                case 8:
                    dialogue.text = "Press A on the keyboard or Left trigger on the controller to use your melee attack.";
                    DeactivateClicker();
                    tutorialStage = 1.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue2()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "Not bad.";
        for (int i = 0; i <= 2; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.1f);
                    yield return new WaitForSeconds(0.11f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(2.75f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "That post has fallen as well? Goodness, are people not showing up today or something.";
                    break;
                case 1:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "Luckily for the both of us, you won’t have to wait for this zombie to approach.";
                    break;
                case 2:
                    dialogue.text = "Press D on the keyboard or Right trigger on the controller to use your ranged attack.";
                    DeactivateClicker();
                    tutorialStage = 2.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue3()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "Out of ammo? Luckily it appears you have an infinite supply lying around.";
        for (int i = 0; i <= 3; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "See that bar below the ranged player's health? That lets you know how much ammo you have left.";
                    break;
                case 1:
                    dialogue.text = "Normally you could keep shotting and the weapon and after running out of ammo it would reload on its own.";
                    break;
                case 2:
                    dialogue.text = "But for now, lets practice doing it manually.";
                    break;
                case 3:
                    tutorialStage = 2.75f;
                    dialogue.text = "Press R on the keyboard or B on the controller to reload your weapon.";
                    DeactivateClicker();
                    break;
            }
        }
    }

    IEnumerator dialogue4()
    {
        tutorialStage = 2.8f;
        StartCoroutine(ActivateClicker());
        dialogue.text = "When you manually reload, your ranged weapon will gain a bonus!";
        for (int i = 0; i <= 2; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "A bonus will double that character's first attack's damage.";
                    break;
                case 1:
                    dialogue.text = "Now, finish them off.";
                    break;
                case 2:
                    tutorialPause = false;
                    dialogue.text = "Press D on the keyboard or Right trigger on the controller to use your ranged attack.";
                    DeactivateClicker();
                    tutorialStage = 2.85f;
                    break;
            }
        }
    }

    IEnumerator dialogue5()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "You’re keeping up pretty well, impressive.";
        for (int i = 0; i <= 3; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.5f);
                    yield return new WaitForSeconds(2f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(1.5f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "Guess we can’t get too confident yet, it appears more Zombies have arrived-";
                    break;
                case 1:
                    enemyManager.spawnPoints = new List<Vector2>();
                    enemyManager.spawnPoints.Add(new Vector2(10.3f, -1.8f));
                    dialogue.text = "";
                    DeactivateClicker();
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.5f);
                    yield return new WaitForSeconds(1.5f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(1.5f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "Okay hmm. This shouldn't be a problem for you right?";
                    break;
                case 2:
                    dialogue.text = "After all, you have something special for situations like these.";
                    break;
                case 3:
                    DeactivateClicker();
                    dialogue.text = "Wait for it...";
                    tutorialPause = false;
                    yield return new WaitForSeconds(3.75f);
                    tutorialPause = true;
                    dialogue.text = "Press Space on the keyboard or A on the controller to swap.";
                    tutorialStage = 3.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue6()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "There you go!";
        while (!progress.IsPressed())
        {
            yield return null;
        }
        while (!progress.WasReleasedThisFrame())
        {
            yield return null;
        }
        for (int i = 0; i <= 5; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "That bar below the duo determines when you're able to do that swap.";
                    break;
                case 1:
                    dialogue.text = "Swapping is also what triggers your melee weapon's bonus.";
                    break;
                case 2:
                    dialogue.text = "However, swapping can also affect your attack controls! Your attack settings can be changed in the main menu or the pause menu.";
                    break;
                case 3:
                    dialogue.text = "While you can't pause in this training session, during an actual shift you would press Enter on pc or Menu on controller to pause.";
                    break;
                case 4:
                    dialogue.text = "Now, with them pushed back from your swap, it's time to end this swarm.";
                    break;
                case 5:
                    tutorialStage = 3.75f;
                    dialogue.text = "Use your melee attack, ranged attack, and swap to defeat the zombies.";
                    DeactivateClicker();
                    tutorialPause = false;
                    break;
            }
        }
    }

    IEnumerator dialogue7()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "Not bad, not bad at all.";
        for (int i = 0; i <= 9; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "Now, lets start giving you some real supplies to work with.";
                    break;
                case 1:
                    PlayerManager.Instance.GiftPlayers(true);
                    dialogue.text = "Killing zombies with a character will slowly upgrade the opposite characters' weapon until you get something new.";
                    break;
                case 2:
                    dialogue.text = "So try your best to have both characters engage in battle so you can upgrade both weapons.";
                    break;
                case 3:
                    DeactivateClicker();
                    dialogue.text = "";
                    tutorialPause = false;
                    for (int j = 0; j < 3; j++)
                    {
                        enemyManager.SpawnEnemyType(0, new Vector2(-10.3f, -1.8f));
                        enemyManager.SpawnEnemyType(0, new Vector2(10.3f, -1.8f));
                        yield return new WaitForSeconds(0.75f);
                    }
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "Looks like the stranglers of that last swarm have finally arrived. Thankfully I only got one last thing to tell you.";
                    break;
                case 5:
                    dialogue.text = "Each character has an ability that I will supply to you in a moment. Caltrops and healing.";
                    break;
                case 6:
                    dialogue.text = "The icons above your players will light up when you can use them, and darken when they are on cooldown.";
                    break;
                case 7:
                    dialogue.text = "You won't be able to act while using an ability, so be cautious when you deploy them.";
                    break;
                case 8:
                    dialogue.text = "On keyboard; H and K are you ability buttons. On controller you'll use; Left and Right Bumber.";
                    break;
                case 9:
                    dialogue.text = "Now, show them what you got!";
                    break;
                case 10:
                    PlayerManager.Instance.GiftPlayers(false);
                    DeactivateClicker();
                    tutorialPause = false;
                    tutorialStage = 4.5f;
                    dialogue.text = "Kill the remaining zombies.";
                    break;
            }
        }
    }

    IEnumerator dialogue8()
    {
        StartCoroutine(ActivateClicker());
        dialogue.text = "You cleaned up well.";
        for (int i = 0; i <= 1; i++)
        {
            while (!progress.IsPressed())
            {
                yield return null;
            }
            while (!progress.WasReleasedThisFrame())
            {
                yield return null;
            }
            switch (i)
            {
                case 0:
                    dialogue.text = "You'll only encounter stronger zombies from this point on, so I wish the best of luck with your shift.";
                    break;
                case 1:
                    dialogue.text = "";
                    DeactivateClicker();
                    MainManager.Instance.ChangeStageRelatively(-1);
                    break;
            }
        }
    }

    IEnumerator spawn()
    {
        enemyManager.AdjustSpawning(0.1f);
        yield return new WaitForSeconds(0.11f);
        enemyManager.AdjustSpawning(0);
    }

    IEnumerator ActivateClicker()
    {
        idle = true;
        reminder.text = "Press Space on the keyboard or A on the controller to progress dialogue.";
        clicker.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 1);
        while (idle)
        {
            clicker.style.translate = new Translate(-2, 0, 0);
            yield return new WaitForSeconds(0.25f);
            clicker.style.translate = new Translate(2, 0, 0);
            yield return new WaitForSeconds(0.25f);
        }
    }

    void DeactivateClicker()
    {
        idle = false;
        reminder.text = "";
        clicker.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0);
    }
}
