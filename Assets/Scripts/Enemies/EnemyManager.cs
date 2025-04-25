using UnityEngine;


// public GameManager gameManager

// Singleton pattern
public class EnemyManager : MonoBehaviour
{

    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

}
