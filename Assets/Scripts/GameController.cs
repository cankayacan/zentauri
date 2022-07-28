using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (GameAudio))]
public class GameController : Singleton<GameController>
{
    private bool gameFinished;

    private GameAudio gameAudio;

    public event Action Goal;

    public event Action Out;

    public override void Awake()
    {
        base.Awake();
        gameAudio = GetComponent<GameAudio>();
    }

    public void RestartGame()
    {
        gameFinished = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PublishGoal()
    {
        if (!gameFinished)
        {
            Goal?.Invoke();
            gameAudio.PlayGoalAudioClip();
        }

        gameFinished = true;
    }

    public void PublishOut()
    {
        if (!gameFinished)
        {
            Out?.Invoke();
            gameAudio.PlayOutAudioClip();
        }
        gameFinished = true;
    }
}
