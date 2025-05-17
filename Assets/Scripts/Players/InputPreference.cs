using UnityEngine;
using UnityEngine.UIElements;

public class InputPreference : MonoBehaviour
{
    [SerializeField] Sprite mR;
    [SerializeField] Sprite lR;

    private Button swap;
    public static bool meleeRanged = true;

    public void Start()
    {
        swap = GetComponent<UIDocument>().rootVisualElement.Q<Button>("SwapButton");

        swap.clickable.clicked += () => { changePreference(false); };

        changePreference(true);
    }

    public void changePreference(bool set)
    {
        if (set)
        {
            meleeRanged = !meleeRanged;
        }
        if (!meleeRanged)
        {
            swap.style.backgroundImage = new StyleBackground(lR);
        }
        else
        {
            swap.style.backgroundImage = new StyleBackground(mR);
        }
        meleeRanged = !meleeRanged;
    }
}
