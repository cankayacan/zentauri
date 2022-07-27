using System;

public class BallEventAggregator : Singleton<BallEventAggregator>
{
    private bool gameFinished = false;

    public event Action Goal;

    public event Action Out;

    public event Action GoalPost;

    public void ResetGame()
    {
        gameFinished = false;
    }

    public void PublishGoal()
    {
        if (!gameFinished) Goal?.Invoke();
        gameFinished = true;
    }

    public void PublishOut()
    {
        if (!gameFinished) Out?.Invoke();
        gameFinished = true;
    }

    public void PublishGoalPost()
    {
        GoalPost?.Invoke();
    }
}
