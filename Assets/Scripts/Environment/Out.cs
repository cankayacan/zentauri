using UnityEngine;

[RequireComponent(typeof (BallDetector))]
[RequireComponent(typeof (AudioSource))]
public class Out: MonoBehaviour
{
    private AudioSource audioSource;
    private BallDetector ballDetector;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ballDetector = GetComponent<BallDetector>();
        ballDetector.BallTouched += OnBallTouched;
    }

    private void OnDestroy()
    {
        ballDetector.BallTouched -= OnBallTouched;
    }

    private void OnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (GameController.Default.finished) return;

        audioSource.Play();
        GameController.Default.PublishOut();
    }
}