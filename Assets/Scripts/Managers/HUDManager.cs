using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/* 
    HUDManager
    Description:
        Manage the in-game HUD
        

    Singleton implementation based on:
        https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
*/
public class HUDManager : MonoBehaviour
{
    // Sprites
    [SerializeField] Sprite p1A1;
    [SerializeField] Sprite p1A2;
    [SerializeField] Sprite p2A1;
    [SerializeField] Sprite p2A2;

    // Bars
    VisualElement p1Health;
    VisualElement p2Health;
    VisualElement swapBar;
    VisualElement ammoBar;

    // Icons
    List<VisualElement> abilityIcons;

    private UIDocument HUDDocument;
    private static HUDManager HUDMInstance;

    public static HUDManager Instance { get { return HUDMInstance; } }

    public bool initialized = false;

    private PlayerManager playerManagerInstance;

    bool shaking;

    private void Awake()
    {
        InitHUDManager();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerManager.Instance != null && PlayerManager.Instance.initialized);
        playerManagerInstance = PlayerManager.Instance;
        ChangeBars(0, false);
        // Set Up Playing Icons
        ChangeBars(5, false);
        AssignImages();
        ChangeBars(6, false);
    }


    void Update()
    {
        /*HUDDocument.rootVisualElement.Q<Label>("Player1health").text
            = "P1 HP: " + playerManagerInstance.player1.GetHealth().ToString()
            + "| P2 HP: " + playerManagerInstance.player2.GetHealth().ToString();*/
    }

    // Function anybody can call to altar the UI
    public void ChangeBars(float call, bool shake)
    {
        // 1 means healthbar edit
        // 2 means ammo edit
        // 3 means swap change
        switch (call)
        {
            case 0:
                resetBars();
                break;
            case 1:
                ((float, float), (float, float)) healths = playerManagerInstance.GetHealthInfo();
                p1Health.style.width = Length.Percent(Mathf.Lerp(0, 100, healths.Item1.Item2 / healths.Item1.Item1));
                p2Health.style.width = Length.Percent(Mathf.Lerp(0, 100, healths.Item2.Item2 / healths.Item2.Item1));
                break;
            case 2:
                // Shake UI if there is no ammo left? Like, if value == 0 already, shake UI
                (float, float) ammo = playerManagerInstance.GetAmmoInfo();
                ammoBar.style.width = Length.Percent(Mathf.Lerp(0, 100, ammo.Item2 / ammo.Item1));
                if (shake)
                {
                    StartCoroutine(shakeBar(ammoBar, 10f));
                }
                break;
            case 2.5f:
                StartCoroutine(shakeBar(ammoBar, 10f));
                break;
            case 3:
                (float, float) swap = playerManagerInstance.GetSwapInfo();
                swapBar.style.width = Length.Percent(Mathf.Lerp(100, 0, swap.Item2 / swap.Item1));
                break;
            case 4:
                HUDDocument.rootVisualElement.Q<Label>("TimeCount").text = ((int)LevelManager.LMInstance.timeSurvived).ToString();
                break;
            case 5:
                // When you swap, if certain control scheme change UI
                break;
            case 6:
                ((bool, bool), (bool, bool)) abilities = playerManagerInstance.GetAbilityInfo();
                bool[] abilityList = new bool[] { abilities.Item1.Item1, abilities.Item1.Item2, abilities.Item2.Item1, abilities.Item2.Item2 };
                for (int i = 0; i < 4; i++)
                {
                    if (abilityList[i])
                    {
                        abilityIcons[i].style.unityBackgroundImageTintColor = new Color(1, 1, 1);
                    }
                    else
                    {
                        abilityIcons[i].style.unityBackgroundImageTintColor = new Color(0.55f, 0.55f, 0.55f);
                    }
                }
                break;
        }
    }

    // Makes bars empty or full at the beginning
    void resetBars()
    {
        p1Health = HUDDocument.rootVisualElement.Q<VisualElement>("P1Fill");
        p2Health = HUDDocument.rootVisualElement.Q<VisualElement>("P2Fill");
        ammoBar = HUDDocument.rootVisualElement.Q<VisualElement>("AmmoFill");
        swapBar = HUDDocument.rootVisualElement.Q<VisualElement>("SwapFill");

        p1Health.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        p2Health.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        ammoBar.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        swapBar.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        shaking = false;
    }

    // Add the needed sprites to the UI
    void AssignImages()
    {
        abilityIcons = new List<VisualElement>();
        abilityIcons.Add(HUDDocument.rootVisualElement.Q<VisualElement>("P1Ability1"));
        abilityIcons.Add(HUDDocument.rootVisualElement.Q<VisualElement>("P1Ability2"));
        abilityIcons.Add(HUDDocument.rootVisualElement.Q<VisualElement>("P2Ability1"));
        abilityIcons.Add(HUDDocument.rootVisualElement.Q<VisualElement>("P2Ability2"));

        abilityIcons[0].style.backgroundImage = new StyleBackground(p1A1);
        abilityIcons[1].style.backgroundImage = new StyleBackground(p1A2);
        abilityIcons[2].style.backgroundImage = new StyleBackground(p2A1);
        abilityIcons[3].style.backgroundImage = new StyleBackground(p2A2);
    }

    // Shakes the given UI bar
    // TODO: Make shaking a list of bools so multiple things can shake
    IEnumerator shakeBar(VisualElement bar, float shakeAmount)
    {
        if (!shaking)
        {
            shaking = true;
            for (int i = 1; i < 6; i++)
            {
                bar.style.translate = new Translate(shakeAmount / i, 0, 0);
                yield return new WaitForSeconds(0.1f);
                bar.style.translate = new Translate(shakeAmount * -1 / i, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }
            bar.style.translate = new Translate(0, 0, 0);
            shaking = false;
        }
    }

    // does whatever Awake would do
    public void InitHUDManager()
    {
        if (HUDMInstance != null && HUDMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            HUDMInstance = this;
        }
        initialized = true;

        HUDDocument = gameObject.GetComponent<UIDocument>();
    }
}