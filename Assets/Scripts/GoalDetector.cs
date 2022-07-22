using System;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    public event Action Goal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Goal?.Invoke();
        }
    }
}
