using UnityEngine;
using UnityEngine.UIElements;

public class InputPreference : MonoBehaviour
{
    [SerializeField] Sprite mR;
    [SerializeField] Sprite lR;
    [SerializeField] Sprite sPC;
    [SerializeField] Sprite sC;
    [SerializeField] Sprite box;

    private Button swap;
    public static bool meleeRanged = true;

    public void Start()
    {
        swap = GetComponent<UIDocument>().rootVisualElement.Q<Button>("SwapButton");
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("SwapPC").style.backgroundImage = new StyleBackground(sPC);
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("SwapController").style.backgroundImage = new StyleBackground(sC);
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Box").style.backgroundImage = new StyleBackground(box);

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
