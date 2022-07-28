using System;
using UnityEngine;

[RequireComponent(typeof (BallAudio))]
public class Ball : MonoBehaviour
{
    private BallAudio ballAudio;

    public void Awake()
    {
        ballAudio = GetComponent<BallAudio>();
    }

    public void Shoot(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalDetector"))
        {
            GameController.Default.PublishGoal();
        }

        if (other.CompareTag("Out"))
        {
            GameController.Default.PublishOut();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("GoalPost"))
        {
            ballAudio.PlayGoalPostAudioClip();
        }
    }
}
