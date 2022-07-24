using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class PlayerAudio: MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip ballKickAudioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBallKickAudio()
    {
        audioSource.PlayOneShot(ballKickAudioClip);
    }
}
