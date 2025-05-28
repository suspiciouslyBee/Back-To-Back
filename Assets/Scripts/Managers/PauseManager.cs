using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour
{
    protected static PauseManager PauMInstance;
    public static PauseManager Instance { get { return PauMInstance; } }
    public bool initialized = false;

    private UIDocument pauseMenu;
    [SerializeField] Sprite resumeButton;
    [SerializeField] Sprite restartButton;
    [SerializeField] Sprite quitButton;
    [SerializeField] Sprite mR;
    [SerializeField] Sprite lR;

    private Button resume;
    private Button restart;
    private Button quit;
    private Button swap;

    public bool isPaused = false;
    private int location;

    public void Start()
    {
        InitPauseManager();
    }

    public void Update()
    {   
        // Temporay pause buttons to test. REMOVE ONCE PAUSE SCREEN IS IMPLEMENTED!!!
        /*
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            UnpauseGame();
        }
        */
    }
    //------------------------- Getters -------------------------
    public bool IsGamePaused()
    {
        /*
        Get if the game is paused

        Inputs:
        * None

        Output:
        * True: game is paused
        * False: game is not paused
        */
        return isPaused;
    }
    //------------------------- Setters -------------------------
    public void PauseGame()
    {
        /*
        Pause the game by setting the time scale to 0 and diplaying the pause screen

        Inputs:
        * None

        Output:
        * None
        */
        isPaused = true;
        DisplayPauseMenu();
    }

    public void UnpauseGame()
    {
        /*
        Unpause the game by setting the time scale to 1 and hiding the pause screen

        Inputs:
        * None

        Output:
        * None
        */
        isPaused = false;
        HidePauseMenu();
        Time.timeScale = 1;
    }
    //------------------------- Actions -------------------------
    private void DisplayPauseMenu()
    {
        /*
        Display the pause screen. Nothing should require timescale to be greater than 0.

        Inputs:
        * None

        Output:
        * None
        */

        highlightButtons(location, true);
        Time.timeScale = 0;
    }

    private void HidePauseMenu()
    {
        /*
        Hide the pause screen.

        Inputs:
        * None

        Output:
        * None
        */

        highlightButtons(0, false);
    }

    public void Movement(bool direction)
    {
        if (isPaused)
        {
            if (direction && location > 0)
            {
                location--;
                highlightButtons(location, true);
            }
            else if (location < 3)
            {
                location++;
                highlightButtons(location, true);
            }
        }
    }

    public void Select()
    {
        if (isPaused)
        {
            switch (location)
            {
                case 0:
                    break;
                case 1:
                    Resume();
                    break;
                case 2:
                    Restart();
                    break;
                case 3:
                    Quit();
                    break;
            }
        }
    }

    private void InitButtons()
    {
        pauseMenu = gameObject.GetComponent<UIDocument>();

        resume = pauseMenu.rootVisualElement.Q<Button>("ResumeButton");
        restart = pauseMenu.rootVisualElement.Q<Button>("RestartButton");
        quit = pauseMenu.rootVisualElement.Q<Button>("QuitButton");
        swap = pauseMenu.rootVisualElement.Q<Button>("SwapButton");

        resume.style.backgroundImage = new StyleBackground(resumeButton);
        restart.style.backgroundImage = new StyleBackground(restartButton);
        quit.style.backgroundImage = new StyleBackground(quitButton);

        resume.RegisterCallback<ClickEvent>(OnResumePressed);
        restart.RegisterCallback<ClickEvent>(OnRestartPressed);
        quit.RegisterCallback<ClickEvent>(OnQuitPressed);

        highlightButtons(0, false);
    }

    private void highlightButtons(int type, bool on)
    {
        resume.SetEnabled(on);
        restart.SetEnabled(on);
        quit.SetEnabled(on);

        if (!on)
        {
            resume.UnregisterCallback<ClickEvent>(OnResumePressed);
            restart.UnregisterCallback<ClickEvent>(OnRestartPressed);
            quit.UnregisterCallback<ClickEvent>(OnQuitPressed);

            resume.style.unityBackgroundImageTintColor = new Color(0, 0, 0, 0);
            restart.style.unityBackgroundImageTintColor = new Color(0, 0, 0, 0);
            quit.style.unityBackgroundImageTintColor = new Color(0, 0, 0, 0);
            pauseMenu.rootVisualElement.Q<VisualElement>("HUD").style.backgroundColor = new Color(0, 0, 0, 0);
        }
        else
        {
            resume.RegisterCallback<ClickEvent>(OnResumePressed);
            restart.RegisterCallback<ClickEvent>(OnRestartPressed);
            quit.RegisterCallback<ClickEvent>(OnQuitPressed);

            resume.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0.3f);
            restart.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0.3f);
            quit.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0.3f);
            pauseMenu.rootVisualElement.Q<VisualElement>("HUD").style.backgroundColor = new Color(0, 0, 0, 100.0f / 255.0f);
        }

        switch (type)
        {
            case 1:
                resume.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 1);
                break;
            case 2:
                restart.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 1);
                break;
            case 3:
                quit.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 1);
                break;

        }
    }

    private void OnResumePressed(ClickEvent evt)
    {
        Resume();
    }

    private void OnRestartPressed(ClickEvent evt)
    {
        Restart();
    }

    private void OnQuitPressed(ClickEvent evt)
    {
        Quit();
    }

    private void Resume()
    {
        UnpauseGame();
    }

    private void Restart()
    {
        UnpauseGame();
        UIManager.Instance.PlayButtonClickSFX();
        MainManager.Instance.ChangeStageRelatively(0);
    }

    private void Quit()
    {
        UnpauseGame();
        MainManager.Instance.ChangeStageRelatively(-2);
    }

    public void InitPauseManager()
    {
        if (PauMInstance != null && PauMInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PauMInstance = this;
            initialized = true;
        }

        location = 1;
        InitButtons();
    }
}
