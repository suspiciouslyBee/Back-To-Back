using System.Collections;
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

    private void Start()
    {
        playerManagerInstance = PlayerManager.Instance;
    }


    void Update()
    {
        /*HUDDocument.rootVisualElement.Q<Label>("Player1health").text
            = "P1 HP: " + playerManagerInstance.player1.GetHealth().ToString()
            + "| P2 HP: " + playerManagerInstance.player2.GetHealth().ToString();*/
    }

    // Function anybody can call to altar the UI
    public void changeBars(int call, bool shake)
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
                HUDDocument.rootVisualElement.Q<VisualElement>("P1Fill").style.width = Length.Percent(Mathf.Lerp(0, 100, healths.Item1.Item2 / healths.Item1.Item1));
                HUDDocument.rootVisualElement.Q<VisualElement>("P2Fill").style.width = Length.Percent(Mathf.Lerp(0, 100, healths.Item2.Item2 / healths.Item2.Item1));
                break;
            case 2:
                // Shake UI if there is no ammo left? Like, if value == 0 already, shake UI
                (float, float) ammo = playerManagerInstance.GetAmmoInfo();
                HUDDocument.rootVisualElement.Q<VisualElement>("AmmoFill").style.width = Length.Percent(Mathf.Lerp(0, 100, ammo.Item2 / ammo.Item1));
                if (shake)
                {
                    StartCoroutine(shakeBar("AmmoBar", 3f));
                }
                break;
            case 3:
                (float, float) swap = playerManagerInstance.GetSwapInfo();
                HUDDocument.rootVisualElement.Q<VisualElement>("SwapFill").style.width = Length.Percent(Mathf.Lerp(0, 100, swap.Item2 / swap.Item1));
                break;
        }
    }

    // Makes bars empty or full at the beginning
    void resetBars()
    {
        HUDDocument.rootVisualElement.Q<VisualElement>("P1Fill").style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        HUDDocument.rootVisualElement.Q<VisualElement>("P2Fill").style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        HUDDocument.rootVisualElement.Q<VisualElement>("AmmoFill").style.width = Length.Percent(Mathf.Lerp(0, 100, 100));
        HUDDocument.rootVisualElement.Q<VisualElement>("SwapFill").style.width = Length.Percent(Mathf.Lerp(0, 100, 0));
        shaking = false;
    }

    // Shakes the given UI bar
    // TODO: Make shaking a list of bools so multiple things can shake
    IEnumerator shakeBar(string bar, float shakeAmount)
    {
        if (!shaking)
        {
            shaking = true;
            Debug.Log(HUDDocument.rootVisualElement.Q<VisualElement>(bar).style.translate.value.y);
            for (int i = 1; i < 6; i++)
            {
                HUDDocument.rootVisualElement.Q<VisualElement>(bar).style.translate = new Translate(shakeAmount / i, 0, 0);
                yield return new WaitForSeconds(0.1f);
                HUDDocument.rootVisualElement.Q<VisualElement>(bar).style.translate = new Translate(shakeAmount * -1 / i, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }
            HUDDocument.rootVisualElement.Q<VisualElement>(bar).style.translate = new Translate(0, 0, 0);
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
        changeBars(0, false);
    }
}