using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof (GameController))]
public class GameScreen : MonoBehaviour
{
    private VisualElement rootVisualElement;
    private GameController gameController;

    private void Awake()
    {
        gameController = GetComponent<GameController>();

        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.style.display = DisplayStyle.None;

        var restartButton = rootVisualElement.Q<Button>("restart-button");
        restartButton.RegisterCallback<ClickEvent>(HandleRestartButtonClick);

        BallEventAggregator.Default.Goal += OnGoal;
        BallEventAggregator.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        BallEventAggregator.Default.Goal -= OnGoal;
        BallEventAggregator.Default.Out -= OnOut;
    }

    private void HandleRestartButtonClick(ClickEvent evt)
    {
        gameController.Restart();
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
