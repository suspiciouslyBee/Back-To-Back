using UnityEngine;
/* 
    UIManager
    Description:
        Manages all UI documents within the game.
        

    Singleton implementation based on:
        https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
*/
public class UIManager : MonoBehaviour
{
    private static UIManager UIMInstance;
    public static UIManager Instance { get { return UIMInstance; } }

    private void Awake()
    {
        if (UIMInstance != null && UIMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            UIMInstance = this;
        }
    }

}
