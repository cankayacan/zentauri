using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class BallAudio: MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip goalPostAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGoalPostAudioClip()
    {
        audioSource.PlayOneShot(goalPostAudioClip);
    }
}
