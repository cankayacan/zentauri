using UnityEngine;

public class Ball : MonoBehaviour
{
    public void Shoot(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
