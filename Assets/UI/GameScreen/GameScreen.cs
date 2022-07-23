using System;
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

        BallEventAggregator.Default.Goal += OnGoal;
    }

    private void OnDestroy()
    {
        BallEventAggregator.Default.Goal -= OnGoal;
    }

    private void HandleRestartButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGoal()
    {
        rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
