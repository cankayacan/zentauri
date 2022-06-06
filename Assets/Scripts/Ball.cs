using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const int VacuumForce = 300;

    private Vector3? shootDestination;
    private Rigidbody ballRigidbody;
    private Vector3 initialPosition;
    
    public void Shoot(Vector3 force, Vector3 destination)
    {
        ballRigidbody = GetComponent<Rigidbody>();
        shootDestination = destination;
        GetComponent<Rigidbody>().AddForce(force);
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (shootDestination == null)
        {
            return;
        }

        var position = transform.position;
        var force = (position - shootDestination.Value) * (-1 * VacuumForce * Time.deltaTime);
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
        }
        
        Debug.Log("OnCollisionEnter");
        shootDestination = null;
    }
}
