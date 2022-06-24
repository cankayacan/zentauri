using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const int VacuumForce = 8000;

    private Vector3? shootDestination;
    private Rigidbody ballRigidbody;
    private Vector3 initialPosition;
    private Vector3 middleDestination;

    public void Shoot(Vector3 force, Vector3 destination)
    {
        ballRigidbody = GetComponent<Rigidbody>();
        shootDestination = destination;
        middleDestination = Vector3.Lerp(initialPosition, destination, 0.5f);
        Debug.Log($"Middle destination {middleDestination} initial {initialPosition} destination {destination}");
        GetComponent<Rigidbody>().AddForce(force);
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (shootDestination == null)
        {
            return;
        }

        var position = transform.position;
        var distanceToMiddle = Math.Abs((position - middleDestination).z);
        var totalDistance = Math.Abs((initialPosition - shootDestination.Value).z) / 2;
        var vacuumForce = (1 - distanceToMiddle / totalDistance) * VacuumForce * Time.fixedDeltaTime;
        Debug.Log($"Vacuum force {position.z} {vacuumForce}");
        var vacuumDirection = (shootDestination.Value - position).normalized;
        var force = vacuumDirection * vacuumForce;
        ballRigidbody.AddForce(force);
        Debug.DrawLine(shootDestination.Value, position);
    }

    void OnCollisionEnter()
    {
        if (shootDestination != null)
        {
            transform.position = initialPosition;
            ballRigidbody.drag = 0;
            ballRigidbody.angularDrag = 0;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }
        
        Debug.Log("OnCollisionEnter");
        shootDestination = null;
    }
}
