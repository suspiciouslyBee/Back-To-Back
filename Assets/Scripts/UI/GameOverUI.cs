using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private UIDocument gameOverDoc;
    private VisualElement gameOverUI;


    // handles GameOver document initialization
    public void InitGameOver()
    {
        if (gameOverDoc != null)
        {
            gameOverUI = gameOverDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        }
        else
        {
            throw new NullReferenceException("No Game Over UI Document found in scene!");
        }

        gameOverUI.style.display = DisplayStyle.None;

        InitButtons();
    }

    public void GameOverSequence()
    {
        gameOverUI.style.display = DisplayStyle.Flex;
    }

    private void InitButtons()
    {

    }
}
