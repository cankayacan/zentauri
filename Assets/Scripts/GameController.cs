using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (GameAudio))]
public class GameController: MonoBehaviour
{
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
        BallEventAggregator.Default.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGoal()
    {
        gameAudio.PlayGoalAudioClip();
    }

    private void OnOut()
    {
        gameAudio.PlayOutAudioClip();
    }
}
