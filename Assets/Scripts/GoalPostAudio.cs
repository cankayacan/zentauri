using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class GoalPostAudio: MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        BallEventAggregator.Default.GoalPost += OnGoalPost;
    }
    
    private void OnDestroy()
    {
        BallEventAggregator.Default.GoalPost -= OnGoalPost;
    }

    private void OnGoalPost()
    {
        audioSource.Play();
    }
}
