using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    private VisualElement rootVisualElement;

    private void Awake()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.style.display = DisplayStyle.None;

        var restartButton = rootVisualElement.Q<Button>("restart-button");
        restartButton.RegisterCallback<ClickEvent>(HandleRestartButtonClick);

        GameController.Default.Goal += OnGoal;
        GameController.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        GameController.Default.Goal -= OnGoal;
        GameController.Default.Out -= OnOut;
    }

    private void HandleRestartButtonClick(ClickEvent evt)
    {
        GameController.Default.RestartGame();
    }

    private void OnGoal()
    {
        ShowRestart();
    }

    private void OnOut()
    {
        ShowRestart();
    }

    private void ShowRestart()
    {
        rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
