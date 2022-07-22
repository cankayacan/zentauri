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
}
