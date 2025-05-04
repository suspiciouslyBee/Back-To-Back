using System;
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
    // public PlayerManager PlayerManagerInstance;

    public static HUDManager Instance { get { return HUDMInstance; } }

    public bool initialized = false;

    private PlayerManager playerManagerInstance;

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
        HUDDocument.rootVisualElement.Q<Label>("Player1health").text
            = "P1 HP: " + playerManagerInstance.player1.GetHealth().ToString()
            + "| P2 HP: " + playerManagerInstance.player2.GetHealth().ToString();
    }

    // currentHealth / maxHealth

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