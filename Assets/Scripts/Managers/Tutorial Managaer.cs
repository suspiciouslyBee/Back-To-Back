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

    public bool tutorialPause;
    public float tutorialStage;

    //The extra checks are here incase there is a duplicate by any means
    private void Awake()
    {
        if (TMInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        TMInstance = this;

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
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "O-Oh It’s you two?! I didn’t realize your post was still…nevermind.";
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Considering you’ve reached out to me, you’re looking for some guidance huh.";
                    break;
                case 1:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Well luckily, I’ve become quite the zombie killing pro over the years. So with some time and training I’m sure you’ll be-";
                    break;
                case 2:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "";
                    enemyManager.AdjustSpawning(0.1f);
                    yield return new WaitForSeconds(0.11f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(2.75f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "What! Have the other posts fallen already?";
                    break;
                case 3:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "It shouldn’t be an issue. I’ll just have to explain things, quicker than I anticipated.";
                    break;
                case 4:
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "You see those two mercenaries at the center of the screen? That's where you are located.";
                    break;
                case 5:
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "The person on the left uses melee weapons, and the person of the right uses ranged weapons.";
                    break;
                case 6:
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "At the top of the screen are your health bars.";
                    break;
                case 7:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = ".";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = ". .";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = ". . .";
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "That zombie's getting uncomfortably close aren't they. Let's change that.";
                    break;
                case 8:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Press A on the keyboard or Left bumper on the controller to use your melee attack.";
                    tutorialStage = 1.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue2()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Not bad for a rookie.";
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "";
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.1f);
                    yield return new WaitForSeconds(0.11f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(2.75f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "That post has fallen as well! Goodness, are people not showing up or something.";
                    break;
                case 1:
                    tutorialPause = false;
                    yield return new WaitForSeconds(1f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "This time however, you won’t have to wait for the zombie to approach";
                    break;
                case 2:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Press S on the keyboard or Right bumper on the controller to use your ranged attack.";
                    tutorialStage = 2.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue3()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Out of ammo? Luckily it appears you have an infinite supply lying around.";
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "See that bar below the ranged player's health? That lets you know how much ammo you have left.";
                    break;
                case 1:
                    tutorialStage = 2.75f;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Press R on the keyboard or B on the controller to reload your gun.";
                    break;
            }
        }
    }

    IEnumerator dialogue4()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Now let's finish them off!";
        while (!progress.IsPressed())
        {
            yield return null;
        }
        while (!progress.WasReleasedThisFrame())
        {
            yield return null;
        }
        tutorialPause = false;
        tutorialStage = 2.85f;
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Press S on the keyboard or Right bumper on the controller to use your ranged attack.";
    }

    IEnumerator dialogue5()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "You’re keeping up pretty well so far for claiming to need my help.";
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "";
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.5f);
                    yield return new WaitForSeconds(2f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(1.5f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Guess we can’t get too confident yet, it appears more Zombies have arrived.";
                    break;
                case 1:
                    enemyManager.spawnPoints = new List<Vector2>();
                    enemyManager.spawnPoints.Add(new Vector2(10.3f, -1.8f));
                    tutorialPause = false;
                    enemyManager.AdjustSpawning(0.5f);
                    yield return new WaitForSeconds(1.5f);
                    enemyManager.AdjustSpawning(0);
                    yield return new WaitForSeconds(1.5f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Okay...that's even more than I thought there'd be.";
                    break;
                case 2:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Oh? You don’t seem to be in much of a panic? You must have something special planned.";
                    break;
                case 3:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "";
                    tutorialPause = false;
                    yield return new WaitForSeconds(3.25f);
                    tutorialPause = true;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Press Space on the keyboard or A on the controller to swap.";
                    tutorialStage = 3.5f;
                    break;
            }
        }
    }

    IEnumerator dialogue6()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Heh, impressive.";
        while (!progress.IsPressed())
        {
            yield return null;
        }
        while (!progress.WasReleasedThisFrame())
        {
            yield return null;
        }
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "It appears that bar between your health bars determines when you're able to do that swap move of yours.";
                    break;
                case 1:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "In that case, with them pushed back from your swap, it's time to take the advantage!";
                    break;
                case 2:
                    tutorialStage = 3.75f;
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Use your weapons and swapping to defeat the remaining zombies.";
                    tutorialPause = false;
                    break;
            }
        }
    }

    IEnumerator dialogue7()
    {
        Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "Not bad, not bad at all.";
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
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "It looks like that was the last of them as well, for now.";
                    break;
                case 1:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "I wish you the best of luck with the rest of your shift.";
                    break;
                case 2:
                    Dialogue.rootVisualElement.Q<Label>("Dialogue").text = "";
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
}
