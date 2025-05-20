using UnityEngine;


// Helpful tools for filming the trailer
/*
    Controls:
    T: increase time survived by 30 seconds
    Y: decrease time survived by 30 seconds
    U: toggle visibility of the HUD;
    I: remove all enemies from the scene.
*/
public class TrailerTools : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        // increase time survived
        if (Input.GetKeyDown(KeyCode.T))
        {
            LevelManager.LMInstance.IncreaseTime(30f);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            LevelManager.LMInstance.IncreaseTime(-30f);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            HUDManager.Instance.ToggleHUD();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            EnemyManager.Instance.RemoveAllEnemies();
        }

    }
}
