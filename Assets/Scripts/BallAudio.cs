using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class BallAudio: MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip goalPostAudioClip;
    [SerializeField] private AudioClip goalAudioClip;
    [SerializeField] private AudioClip outAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGoalPostAudioClip()
    {
        audioSource.PlayOneShot(goalPostAudioClip);
    }

    public void PlayGoalAudioClip()
    {
        audioSource.PlayOneShot(goalAudioClip);
    }

    public void PlayOutAudioClip()
    {
        audioSource.PlayOneShot(outAudioClip);
    }
}
