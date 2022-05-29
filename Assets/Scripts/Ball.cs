using UnityEngine;

public class Ball : MonoBehaviour
{
    private int vacuumForce = 300;
    private Vector3? shootDestination;
    
    public void Shoot(Vector3 force, Vector3 destination)
    {
        shootDestination = destination;
        GetComponent<Rigidbody>().AddForce(force);
    }
    
    private void Update()
    {
        if (shootDestination == null)
        {
            return;
        }

        var position = transform.position;
        var force = -1 * (position - shootDestination.Value) * vacuumForce * Time.deltaTime;
        // Debug.Log($"Update {force}");
        GetComponent<Rigidbody>().AddForce(force);
        Debug.DrawLine(shootDestination.Value, position);
    }
    
    void OnCollisionEnter()
    {
        Debug.Log("OnCollisionEnter");
        shootDestination = null;
    }
}
