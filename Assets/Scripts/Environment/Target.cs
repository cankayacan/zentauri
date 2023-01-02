using UnityEngine;

[RequireComponent(typeof (BallDetector))]
[RequireComponent(typeof (AudioSource))]
public class Target: MonoBehaviour
{
    private AudioSource audioSource;
    private BallDetector ballDetector;
    private ParticleSystem dissolveEffect;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        dissolveEffect = GetComponentInChildren<ParticleSystem>();
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
        meshRenderer.enabled = false;
        dissolveEffect.Play();
    }
}
