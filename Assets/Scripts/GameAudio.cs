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
        
        BallEventAggregator.Default.Goal += OnGoal;
        BallEventAggregator.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        BallEventAggregator.Default.Goal -= OnGoal;
        BallEventAggregator.Default.Out -= OnOut;
    }

    private void OnGoal()
    {
        audioSource.PlayOneShot(goalAudioClip);
    }

    private void OnOut()
    {
        audioSource.PlayOneShot(outAudioClip);
    }
}
