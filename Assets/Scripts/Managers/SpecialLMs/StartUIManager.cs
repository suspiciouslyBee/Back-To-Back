using UnityEngine;
using UnityEngine.UIElements;

public class StartUIManager : UIManager
{
    [SerializeField]
    private UIDocument mainMenu;

    [SerializeField] Sprite endlessButton;
    [SerializeField] Sprite tutorialButton;

    [SerializeField] Sprite lC;
    [SerializeField] Sprite lPC;
    [SerializeField] Sprite rC;
    [SerializeField] Sprite rPC;

    private Button startButton;
    private Button trainingButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void InitAll()
    {
        //register the buttons to our special LM
        startButton = mainMenu.rootVisualElement.Q<Button>("StartGame");
        startButton.style.backgroundImage = new StyleBackground(endlessButton);
        startButton.RegisterCallback<ClickEvent>(OnStartButtonPressed);

        trainingButton = mainMenu.rootVisualElement.Q<Button>("Training");
        trainingButton.style.backgroundImage = new StyleBackground(tutorialButton);
        trainingButton.RegisterCallback<ClickEvent>(OnTrainingButtonPressed);

        mainMenu.rootVisualElement.Q<VisualElement>("LeftController").style.backgroundImage = new StyleBackground(lC);
        mainMenu.rootVisualElement.Q<VisualElement>("LeftPC").style.backgroundImage = new StyleBackground(lPC);
        mainMenu.rootVisualElement.Q<VisualElement>("RightController").style.backgroundImage = new StyleBackground(rC);
        mainMenu.rootVisualElement.Q<VisualElement>("RightPC").style.backgroundImage = new StyleBackground(rPC);

    }

    private void OnStartButtonPressed(ClickEvent evt)
    {
        PlayButtonClickSFX();
        MainManager.Instance.ChangeStageRelatively(2);
    }

    private void OnTrainingButtonPressed(ClickEvent evt)
    {
        PlayButtonClickSFX();
        MainManager.Instance.ChangeStageRelatively(1);
    }
}
