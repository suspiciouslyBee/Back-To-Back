using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnGUI()
    {
        // TODO: write to controls
        GUI.Label(new Rect(10, 10, 50, 500), $"Controls: ");
        GUI.Label(new Rect(10, 60, 50, 500), $"");
        GUI.Label(new Rect(10, 110, 50, 500), $"");
    }
}