using System;

public class BallEventAggregator : Singleton<BallEventAggregator>
{
    public event Action Goal;

    public event Action Out;

    public event Action GoalPost;

    public void PublishGoal()
    {
        Goal?.Invoke();
    }
    
    public void PublishOut()
    {
        Out?.Invoke();
    }
    
    public void PublishGoalPost()
    {
        GoalPost?.Invoke();
    }
}
