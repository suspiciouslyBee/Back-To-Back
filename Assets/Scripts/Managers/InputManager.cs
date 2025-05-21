/*
 * Input Manager
 * 
 * This handles input actions sepcified in the unity input system package
 * 
 * may or maynot make this a singleton by composition in MM
*/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour
{

    //Chances are I may refactor this into an list or something

    //these should be readable by the PC if the PC really needs to see extra data about the IA
    InputAction swapCharacters;

    InputAction fireGun;
    InputAction meleeAbility1;
    InputAction meleeAbility2;
    InputAction reloadGun;
    InputAction rangedAbility1;
    InputAction rangedAbility2;
    InputAction swingSword;
    InputAction restartLevel;
    LevelManager LMinstance;

    bool swapDown;
    bool reloadDown;
    bool mA1Down;
    bool mA2Down;
    bool rA1Down;
    bool rA2Down;

    // called second
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swapCharacters = InputSystem.actions.FindAction("SwapCharacters");
        fireGun = InputSystem.actions.FindAction("RangedAttack");
        meleeAbility1 = InputSystem.actions.FindAction("Ability1");
        meleeAbility2 = InputSystem.actions.FindAction("Ability2");
        reloadGun = InputSystem.actions.FindAction("RangedReload");
        rangedAbility1 = InputSystem.actions.FindAction("Ability3");
        rangedAbility2 = InputSystem.actions.FindAction("Ability4");
        swingSword = InputSystem.actions.FindAction("MeleeAttack");
        restartLevel = InputSystem.actions.FindAction("RestartLevel");

        swapDown = false;
        reloadDown = false;
        mA1Down = false;
        mA2Down = false;
        rA1Down = false;
        rA2Down = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            StartCoroutine(ChangedActiveScene());
        }
    }

    //refresh the PC
    IEnumerator ChangedActiveScene()
    {
        yield return new WaitUntil(() => MainManager.Instance != null);
        LMinstance = MainManager.Instance.GetLevelManager();
    }

    //for now using actions workflow, may consider actions & playerinput workflow sometime
    // Update is called once per frame
    //its possible this abstraction is unneccesary, but LM could be overridden with custom stuff
    //so we basically bind the actions to a quazi interface here

    void Update()
    {
        if (swapCharacters.ReadValue<float>() == 0 && swapDown)
        {
            swapDown = false;
        }
        if (swapCharacters.ReadValue<float>() > 0 && !swapDown)
        {
            swapDown = true;
            LMinstance.SwapChars();
        }
        if (fireGun.IsPressed())
        {
            LMinstance.FireGun();
        }
        if (reloadGun.ReadValue<float>() == 0 && reloadDown)
        {
            reloadDown = false;
        }
        if (reloadGun.ReadValue<float>() > 0 && !reloadDown)
        {
            reloadDown = true;
            LMinstance.ReloadGun();
        }
        if (swingSword.IsPressed())
        {
            LMinstance.SwingSword();
        }
        if (meleeAbility1.ReadValue<float>() == 0 && mA1Down)
        {
            mA1Down = false;
        }
        if (meleeAbility1.ReadValue<float>() > 0 && !mA1Down)
        {
            mA1Down = true;
            LMinstance.MeleeAbility1();
        }
        if (meleeAbility2.ReadValue<float>() == 0 && mA2Down)
        {
            mA2Down = false;
        }
        if (meleeAbility2.ReadValue<float>() > 0 && !mA2Down)
        {
            mA2Down = true;
            LMinstance.MeleeAbility2();
        }
        if (reloadGun.ReadValue<float>() > 0 && !reloadDown)
        {
            reloadDown = true;
            LMinstance.ReloadGun();
        }
        if (rangedAbility1.ReadValue<float>() == 0 && rA1Down)
        {
            rA1Down = false;
        }
        if (rangedAbility1.ReadValue<float>() > 0 && !rA1Down)
        {
            rA1Down = true;
            LMinstance.RangedAbility1();
        }
        if (rangedAbility2.ReadValue<float>() == 0 && rA2Down)
        {
            rA2Down = false;
        }
        if (rangedAbility2.ReadValue<float>() > 0 && !rA2Down)
        {
            rA2Down = true;
            LMinstance.RangedAbility2();
        }
        /*if (restartLevel.IsPressed())
        {
            LMinstance.RestartLevel();
        }*/
    }
}
