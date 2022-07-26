using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (GameAudio))]
public class GameController: MonoBehaviour
{
    private bool isPlaying = true;

    private GameAudio gameAudio;

    private void Awake()
    {
        gameAudio = GetComponent<GameAudio>();

        BallEventAggregator.Default.Goal += OnGoal;
        BallEventAggregator.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        BallEventAggregator.Default.Goal -= OnGoal;
        BallEventAggregator.Default.Out -= OnOut;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGoal()
    {
        if (isPlaying) gameAudio.PlayGoalAudioClip();
        isPlaying = false;
    }

    private void OnOut()
    {
        if (isPlaying) gameAudio.PlayOutAudioClip();
        isPlaying = false;
    }
}
