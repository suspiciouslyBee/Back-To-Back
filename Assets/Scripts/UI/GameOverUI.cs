using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private UIDocument doc;
    private VisualElement gameOverUI;

    [SerializeField] Sprite rB;
    [SerializeField] Sprite qB;

    // handles GameOver document initialization
    public void InitGameOver()
    {
        if (doc != null)
        {
            // gameOverUI = doc.rootVisualElement.Q<VisualElement>("MainContainer");
        }
        else
        {
            throw new NullReferenceException("No Game Over UI Document found in scene!");
        }

        // gameOverUI.style.display = DisplayStyle.None;

        // InitButtons();
    }

    public void OnDisable()
    {
        DisableButtons();
    }

    public void GameOverSequence()
    {
        gameOverUI.style.display = DisplayStyle.Flex;
    }

    private void OnRestartPressed(ClickEvent evt)
    {
        UIManager.Instance.PlayButtonClickSFX();
        MainManager.Instance.ChangeStageRelatively(0);
    }

    private void OnQuitPressed(ClickEvent evt)
    {
        MainManager.Instance.ChangeStageRelatively(-2);
    }

    private void InitButtons()
    {
        restartButton = doc.rootVisualElement.Q<Button>("restart-button");
        restartButton.RegisterCallback<ClickEvent>(OnRestartPressed);
        restartButton.style.backgroundImage = new StyleBackground(rB);

        quitButton = doc.rootVisualElement.Q<Button>("quit-button");
        quitButton.RegisterCallback<ClickEvent>(OnQuitPressed);
        quitButton.style.backgroundImage = new StyleBackground(qB);
    }

    // disable all callbacks because good practice
    private void DisableButtons()
    {
        //    restartButton.UnregisterCallback<ClickEvent>(OnRestartPressed);
        quitButton.UnregisterCallback<ClickEvent>(OnQuitPressed);
    }

    // private fields
    Button restartButton;
    Button quitButton;
}
