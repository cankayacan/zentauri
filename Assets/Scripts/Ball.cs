using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public void Shoot(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalDetector"))
        {
            BallEventAggregator.Default.PublishGoal();
        }

        if (other.CompareTag("Out"))
        {
            BallEventAggregator.Default.PublishOut();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("GoalPost"))
        {
            BallEventAggregator.Default.PublishGoalPost();
        }
    }
}
