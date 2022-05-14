using BezierSolution;
using Unity.VisualScripting;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    [SerializeField] private int DURATION = 5;

    private Rigidbody ballRigidbody;
    private BezierSpline spline;

    private float time;
    private float force = 7f;
    private int pointIndex = -1;
    private bool movementComplete;
    private Vector3 heading;


    public void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        spline = GetComponent<BezierSpline>();
        Vector3 start = spline.GetPoint(0);
        Vector3 end = spline.GetPoint(1);
        Debug.Log($"Start {start} {end}");

        ballRigidbody.velocity = (end - start).normalized * force;
    }

    public void Update()
    {
        // time += Time.deltaTime;
        // var normTime = (time % DURATION) / DURATION;
        // ball.transform.position = spline.GetPoint(normTime);
    }

    void FixedUpdate()
    {
        MoveBall();
    }


    void MoveBall()
    {
        time += Time.fixedDeltaTime;
        var normTime = (time % DURATION) / DURATION;

        var intendedPosition = spline.GetPoint(normTime);

        // var offset = Vector3.Distance(intendedPosition, ball.transform.position);
        heading = (intendedPosition - ball.transform.position).normalized;
        ballRigidbody.velocity = heading * force;

        // Debug.Log($"Offset {offset}");
        //
        // if (offset >= 0.2f)
        // {
        //     heading = (intendedPosition - ball.transform.position).normalized;
        //     ballRigidbody.velocity = heading * force;
        // }
    }
}
