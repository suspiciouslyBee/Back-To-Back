/*
 * Input Manager
 * 
 * This handles input actions sepcified in the unity input system package
 * 
 * may or maynot make this a singleton by composition in MM
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    //Chances are I may refactor this into an list or something
    InputAction swapCharacters;
    
    InputAction fireLeft;
    InputAction reloadLeft;
    InputAction fireRight;
    InputAction reloadRight;

    InputAction restartLevel;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
