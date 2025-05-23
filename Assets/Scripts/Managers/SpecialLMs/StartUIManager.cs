using UnityEngine;
using UnityEngine.UIElements;

public class StartUIManager : UIManager
{
    [SerializeField]
    private UIDocument mainMenu;

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
        startButton.RegisterCallback<ClickEvent>(OnStartButtonPressed);

        trainingButton = mainMenu.rootVisualElement.Q<Button>("Training");
        trainingButton.RegisterCallback<ClickEvent>(OnTrainingButtonPressed);

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
