using System;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public event Action Finished;

    public void RestartGame()
    {
        GameController.Default.RestartGame();
    }

    private void Awake()
    {
        GameController.Default.Goal += OnFinished;
        GameController.Default.Out += OnFinished;
    }

    private void OnDestroy()
    {
        GameController.Default.Goal -= OnFinished;
        GameController.Default.Out -= OnFinished;
    }

    private void OnFinished()
    {
        Finished?.Invoke();
    }
}