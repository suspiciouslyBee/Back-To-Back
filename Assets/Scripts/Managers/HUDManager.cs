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
    // Ability Sprites
    [SerializeField] Sprite p1A1;
    [SerializeField] Sprite p1A2;
    [SerializeField] Sprite p2A1;
    [SerializeField] Sprite p2A2;

    // Button Sprites
    [SerializeField] Sprite leftAttackButton;
    [SerializeField] Sprite rightAttackButton;
    [SerializeField] Sprite swapButton;
    [SerializeField] Sprite reloadButton;
    [SerializeField] Sprite leftAbility1Button;
    [SerializeField] Sprite leftAbility2Button;
    [SerializeField] Sprite rightAbility1Button;
    [SerializeField] Sprite rightAbility2Button;

    // Symbol Sprites
    [SerializeField] Sprite meleeIcon;
    [SerializeField] Sprite rangedIcon;
    [SerializeField] Sprite reloadIcon;
    [SerializeField] Sprite swapIcon;
    [SerializeField] Sprite leftAbility1Icon;
    [SerializeField] Sprite leftAbility2Icon;
    [SerializeField] Sprite rightAbility1Icon;
    [SerializeField] Sprite rightAbility2Icon;

    // Bars
    VisualElement p1Health;
    VisualElement p2Health;
    VisualElement swapBar;
    VisualElement ammoBar;

    // Icons
    List<VisualElement> abilityIcons;

    // Controls
    VisualElement leftInput;
    VisualElement rightInput;
    VisualElement leftAbility1;
    VisualElement leftAbility2;
    VisualElement rightAbility1;
    VisualElement rightAbility2;

    private UIDocument HUDDocument;
    private static HUDManager HUDMInstance;

    public static HUDManager Instance { get { return HUDMInstance; } }

    public bool initialized = false;

    private PlayerManager playerManagerInstance;

    bool ammoShaking;
    bool swapShaking;

    private void Awake()
    {
        InitHUDManager();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerManager.Instance != null && PlayerManager.Instance.initialized);
        playerManagerInstance = PlayerManager.Instance;
        ChangeBars(0, false);
        AssignControllerSprites();
        AssignSymbolSprites();
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
                ammoBar.Q<VisualElement>("AmmoFill").style.width = Length.Percent(Mathf.Lerp(0, 100, ammo.Item2 / ammo.Item1));
                if (shake && !ammoShaking)
                {
                    StartCoroutine(shakeBar(ammoBar, 5f, 0));
                }
                break;
            case 2.5f:
                if (!ammoShaking)
                {
                    StartCoroutine(shakeBar(ammoBar, 5f, 0));
                }
                break;
            case 3:
                (float, float) swap = playerManagerInstance.GetSwapInfo();
                swapBar.Q<VisualElement>("SwapFill").style.width = Length.Percent(Mathf.Lerp(100, 0, swap.Item2 / swap.Item1));
                break;
            case 3.5f:
                if (!swapShaking)
                {
                    StartCoroutine(shakeBar(swapBar, 10f, 1));
                }
                break;
            case 4:
                HUDDocument.rootVisualElement.Q<Label>("TimeCount").text = ((int)LevelManager.LMInstance.timeSurvived).ToString();
                break;
            case 5:
                // Sword <==> Gun
                StyleBackground temp = leftInput.style.backgroundImage;
                leftInput.style.backgroundImage = rightInput.style.backgroundImage;
                rightInput.style.backgroundImage = temp;

                // MeleeAbility1 <==> RangedAbility1
                temp = leftAbility1.style.backgroundImage;
                leftAbility1.style.backgroundImage = rightAbility1.style.backgroundImage;
                rightAbility1.style.backgroundImage = temp;

                // MeleeAbility2 <==> RangedAbility2
                temp = leftAbility2.style.backgroundImage;
                leftAbility2.style.backgroundImage = rightAbility2.style.backgroundImage;
                rightAbility2.style.backgroundImage = temp;
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
        ammoBar = HUDDocument.rootVisualElement.Q<VisualElement>("AmmoBar");
        swapBar = HUDDocument.rootVisualElement.Q<VisualElement>("SwapBar");

        p1Health.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        p2Health.style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        ammoBar.Q<VisualElement>("AmmoFill").style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        swapBar.Q<VisualElement>("SwapFill").style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        ammoShaking = false;
        swapShaking = false;
    }

    // Add the needed controller sprites to the UI
    void AssignControllerSprites()
    {
        HUDDocument.rootVisualElement.Q<VisualElement>("LeftAttackButton").style.backgroundImage = new StyleBackground(leftAttackButton);
        HUDDocument.rootVisualElement.Q<VisualElement>("RightAttackButton").style.backgroundImage = new StyleBackground(rightAttackButton);
        HUDDocument.rootVisualElement.Q<VisualElement>("SwapButton").style.backgroundImage = new StyleBackground(swapButton);
        HUDDocument.rootVisualElement.Q<VisualElement>("ReloadButton").style.backgroundImage = new StyleBackground(reloadButton);

        HUDDocument.rootVisualElement.Q<VisualElement>("LeftAbility1Button").style.backgroundImage = new StyleBackground(leftAbility1Button);
        HUDDocument.rootVisualElement.Q<VisualElement>("LeftAbility2Button").style.backgroundImage = new StyleBackground(leftAbility2Button);
        HUDDocument.rootVisualElement.Q<VisualElement>("RightAbility1Button").style.backgroundImage = new StyleBackground(rightAbility1Button);
        HUDDocument.rootVisualElement.Q<VisualElement>("RightAbility2Button").style.backgroundImage = new StyleBackground(rightAbility2Button);
    }

    // Add the needed symbol sprites to the UI
    void AssignSymbolSprites()
    {
        leftInput = HUDDocument.rootVisualElement.Q<VisualElement>("LeftIcon");
        rightInput = HUDDocument.rootVisualElement.Q<VisualElement>("RightIcon");
        leftAbility1 = HUDDocument.rootVisualElement.Q<VisualElement>("LeftAbility1Icon");
        leftAbility2 = HUDDocument.rootVisualElement.Q<VisualElement>("LeftAbility2Icon"); ;
        rightAbility1 = HUDDocument.rootVisualElement.Q<VisualElement>("RightAbility1Icon"); ;
        rightAbility2 = HUDDocument.rootVisualElement.Q<VisualElement>("RightAbility2Icon"); ;


        leftInput.style.backgroundImage = new StyleBackground(meleeIcon);
        rightInput.style.backgroundImage = new StyleBackground(rangedIcon);
        HUDDocument.rootVisualElement.Q<VisualElement>("SwapIcon").style.backgroundImage = new StyleBackground(swapIcon);
        HUDDocument.rootVisualElement.Q<VisualElement>("ReloadIcon").style.backgroundImage = new StyleBackground(reloadIcon);
        leftAbility1.style.backgroundImage = new StyleBackground(leftAbility1Icon);
        leftAbility2.style.backgroundImage = new StyleBackground(leftAbility2Icon);
        rightAbility1.style.backgroundImage = new StyleBackground(rightAbility1Icon);
        rightAbility2.style.backgroundImage = new StyleBackground(rightAbility2Icon);
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
    IEnumerator shakeBar(VisualElement bar, float shakeAmount, int type)
    {
        switch (type)
        {
            case 0:
                ammoShaking = true;
                break;
            case 1:
                swapShaking = true;
                break;
        }
        for (int i = 1; i < 6; i++)
        {
            bar.style.translate = new Translate(shakeAmount / i, 0, 0);
            yield return new WaitForSeconds(0.1f);
            bar.style.translate = new Translate(shakeAmount * -1 / i, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        bar.style.translate = new Translate(0, 0, 0);
        switch (type)
        {
            case 0:
                ammoShaking = false;
                break;
            case 1:
                swapShaking = false;
                break;
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

    public void ToggleHUD()
    {
        if (HUDDocument.rootVisualElement.style.display == DisplayStyle.None)
        {
            HUDDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            HUDDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}