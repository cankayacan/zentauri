using UnityEngine;

public class MoveBackAndForth: MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float speed = 1.2f;

    public void Update()
    {
        var time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(start, end, time);
    }
}