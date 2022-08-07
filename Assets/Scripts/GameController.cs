using System;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public bool finished;

    public event Action Goal;

    public event Action Out;

    public void RestartGame()
    {
        finished = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PublishGoal()
    {
        if (!finished)
        {
            Goal?.Invoke();
        }

        finished = true;
    }

    public void PublishOut()
    {
        if (!finished)
        {
            Out?.Invoke();
        }

        finished = true;
    }
}
