using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    private VisualElement rootVisualElement;
    private VisualElement errorContainerElement;
    private Button nextButton;
    private Button restartButton;

    private void Awake()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        errorContainerElement = rootVisualElement.Q<VisualElement>("error-container");
        errorContainerElement.style.display = DisplayStyle.None;

        nextButton = rootVisualElement.Q<Button>("next-button");
        nextButton.style.display = DisplayStyle.None;
        nextButton.RegisterCallback<ClickEvent>(HandleNextButtonClick);
        
        restartButton = rootVisualElement.Q<Button>("restart-button");
        restartButton.style.display = DisplayStyle.None;
        restartButton.RegisterCallback<ClickEvent>(HandleRestartButtonClick);

        GameController.Default.Goal += OnGoal;
        GameController.Default.Out += OnOut;
        GameController.Default.LevelError += OnLevelError;
    }

    private void OnDestroy()
    {
        GameController.Default.Goal -= OnGoal;
        GameController.Default.Out -= OnOut;
        GameController.Default.LevelError -= OnLevelError;
    }

    private void HandleNextButtonClick(ClickEvent evt)
    {
        GameController.Default.NextLevel();
    }

    private void HandleRestartButtonClick(ClickEvent evt)
    {
        GameController.Default.RestartGame();
    }

    private void OnGoal()
    {
        ShowNextButton();
    }

    private void OnOut()
    {
        ShowRestartButton();
    }

    private void OnLevelError(string error)
    {
        errorContainerElement.style.display = DisplayStyle.Flex;

        var errorLabel = rootVisualElement.Q<Label>("error-label");
        errorLabel.text = error;

        ShowRestartButton();
    }

    private void ShowNextButton()
    {
        nextButton.style.display = DisplayStyle.Flex;
        restartButton.style.display = DisplayStyle.None;
    }
    
    private void ShowRestartButton()
    {
        nextButton.style.display = DisplayStyle.None;
        restartButton.style.display = DisplayStyle.Flex;
    }
}
