using UnityEngine;


// Helpful tools for filming the trailer
/*
    Controls:
    T: increase time survived by 30 seconds
    Y: decrease time survived by 30 seconds
    U: toggle visibility of the HUD;
    I: remove all enemies from the scene.
    O: skip a wave
    P: spawn an avengers-level threat
*/
public class TrailerTools : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        // increase time survived
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"[TrailerTools] increasing survival time by 30 seconds (now {LevelManager.LMInstance.timeSurvived} seconds)");
            LevelManager.LMInstance.IncreaseTime(30f);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log($"[TrailerTools] Decreaseing survival time by 30 seconds (now {LevelManager.LMInstance.timeSurvived} seconds)");
            LevelManager.LMInstance.IncreaseTime(-30f);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("[TrailerTools] Toggling HUD");
            HUDManager.Instance.ToggleHUD();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("[TrailerTools] Removing All Enemies");
            EnemyManager.Instance.RemoveAllEnemies();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyManager.Instance.SkipWave();
            Debug.Log($"[TrailerTools] Skipping a wave! Wave is now {EnemyManager.Instance.GetWaveCount()}");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyManager.Instance.SpawnTrailerHorde();
        }

    }
}
