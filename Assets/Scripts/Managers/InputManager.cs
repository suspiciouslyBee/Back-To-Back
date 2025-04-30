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

public class InputManager : MonoBehaviour
{

    //Chances are I may refactor this into an list or something

    //these should be readable by the PC if the PC really needs to see extra data about the IA
    InputAction swapCharacters;

    InputAction fireGun;
    InputAction reloadGun;
    InputAction swingSword;

    InputAction restartLevel;



    LevelManager LMinstance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swapCharacters = InputSystem.actions.FindAction("SwapCharacters");
        fireGun = InputSystem.actions.FindAction("FireGun");
        reloadGun = InputSystem.actions.FindAction("ReloadGun");
        swingSword = InputSystem.actions.FindAction("SwingSword");
        restartLevel = InputSystem.actions.FindAction("RestartLevel");
    }

    //refresh the PC
    void ChangedActiveScene()
    {
        LMinstance = MainManager.Instance.GetLevelManager();
    }


    //for now using actions workflow, may consider actions & playerinput workflow sometime
    // Update is called once per frame
    //its possible this abstraction is unneccesary, but LM could be overridden with custom stuff
    //so we basically bind the actions to a quazi interface here

    void Update()
    {
        if (swapCharacters.IsPressed())
        {
            LMinstance.SwapChars();
        }
        if (fireGun.IsPressed())
        {
            LMinstance.FireGun();
        }
        if (reloadGun.IsPressed())
        {
            LMinstance.ReloadGun();
        }
        if (swingSword.IsPressed())
        {
            LMinstance.SwingSword();
        }
        if (restartLevel.IsPressed())
        {
            LMinstance.SwingSword();
        }

    }


}
