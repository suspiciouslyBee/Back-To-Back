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
                    dialogue.text = "Considering you’ve reached out to me, you’re looking for some guidance huh.";
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
                    dialogue.text = "What! Have the other posts fallen already?";
                    break;
                case 3:
                    dialogue.text = "It shouldn’t be an issue. I’ll just have to explain things, quicker than I anticipated.";
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
                    dialogue.text = "That zombie's getting uncomfortably close aren't they. Let's change that.";
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
        dialogue.text = "Not bad for a rookie.";
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
                    DeactivateClicker();
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.1f);
                    yield return new WaitForSeconds(0.11f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(2.75f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "That post has fallen as well! Goodness, are people not showing up or something.";
                    break;
                case 1:
                    DeactivateClicker();
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    StartCoroutine(ActivateClicker());
                    dialogue.text = "This time however, you won’t have to wait for the zombie to approach";
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
                    dialogue.text = "See that bar below the ranged player's health? That lets you know how much ammo you have left.";
                    break;
                case 1:
                    dialogue.text = "Normally you could keep shotting and the weapon would reload on its own, but for now lets try doing this manually.";
                    break;
                case 2:
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
        dialogue.text = "When you manually reload, your ranged weapon will get a damage bonus!";
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
                    dialogue.text = "This boost will only last one shot though, so make it count.";
                    break;
                case 1:
                    dialogue.text = "Now let's finish them off!";
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
        dialogue.text = "You’re keeping up pretty well so far for claiming to need my help.";
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
                    dialogue.text = "Guess we can’t get too confident yet, it appears more Zombies have arrived.";
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
                    dialogue.text = "Okay...that's even more than I thought there'd be.";
                    break;
                case 2:
                    dialogue.text = "You haven't forgotten how to handle big crowds, have you?";
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
        dialogue.text = "Heh, impressive.";
        while (!progress.IsPressed())
        {
            yield return null;
        }
        while (!progress.WasReleasedThisFrame())
        {
            yield return null;
        }
        for (int i = 0; i <= 4; i++)
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
                    dialogue.text = "That bar below the duo determines when you're able to do that swap move of yours.";
                    break;
                case 1:
                    dialogue.text = "Swapping appears to be what triggers your melee weapon's bonus as well.";
                    break;
                case 2:
                    dialogue.text = "However, swapping can also affect how you attack, which can be altared in the main menu.";
                    break;
                case 3:
                    dialogue.text = "Now, with them pushed back from your swap, it's time to take the advantage!";
                    break;
                case 4:
                    tutorialStage = 3.75f;
                    dialogue.text = "Press A on the keyboard or Left trigger on the controller to use your melee attack. Press D on the keyboard or Right trigger on the controller to use your ranged attack.";
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
                    dialogue.text = "It looks like that was the last of them as well, for now.";
                    break;
                case 1:
                    dialogue.text = "I wish you the best of luck with the rest of your shift.";
                    break;
                case 2:
                    dialogue.text = "";
                    DeactivateClicker();
                    SceneManager.LoadScene("StartScene");
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
