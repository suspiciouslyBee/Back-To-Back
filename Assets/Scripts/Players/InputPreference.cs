using UnityEngine;
using UnityEngine.UIElements;

public class InputPreference : MonoBehaviour
{
    private Button swap;
    public static bool meleeRanged = true;

    public void Start()
    {
        swap = GetComponent<UIDocument>().rootVisualElement.Q<Button>("SwapButton");

        swap.clickable.clicked += () => { changePreference(); };
    }

    public void changePreference()
    {
        meleeRanged = !meleeRanged;
    }
}
