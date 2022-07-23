using UnityEngine;

public class Spinner : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private int speed = 40;

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
