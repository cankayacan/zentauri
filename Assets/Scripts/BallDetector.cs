using System;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    public event Action<GameObject, GameObject> BallTouched;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallTouched?.Invoke(gameObject, other.gameObject);
        }
    }
}
