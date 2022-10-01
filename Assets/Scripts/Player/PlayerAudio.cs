using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class PlayerAudio: MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip walkingAudioClip;

    [SerializeField] private AudioClip ballKickAudioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FootStep()
    {
        if (!walkingAudioClip) return;
        audioSource.PlayOneShot(walkingAudioClip);
    }

    public void PlayBallKickAudio()
    {
        if (!ballKickAudioClip) return;
        audioSource.PlayOneShot(ballKickAudioClip);
    }
}
