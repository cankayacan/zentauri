using System;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class GameAudio: MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip goalAudioClip;
    [SerializeField] private AudioClip outAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
