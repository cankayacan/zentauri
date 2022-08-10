using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BallAudio))]
public class Ball : MonoBehaviour
{
    private BallAudio ballAudio;
    private Rigidbody ballRigidbody;
    public PlayerCharacterController owner;

    private Vector3 cachedPosition;
    private Vector3 cachedVelocity;
    private Vector3 cachedAngularVelocity;

    public Vector3 speed => ballRigidbody.velocity;

    public void Awake()
    {
        ballAudio = GetComponent<BallAudio>();
        ballRigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (owner == null) return;

        Dribble();

        ballRigidbody.angularVelocity = owner.transform.right * owner.speed.magnitude;
    }

    public void Shoot(Vector3 velocity)
    {
        Uncontrol();
        ballRigidbody.velocity = velocity;
    }

    public void Control(PlayerCharacterController characterController)
    {
        if (owner == characterController) return;

        SetVelocityToZero();
        owner = characterController;
    }

    public void Uncontrol()
    {
        if (owner)
        {
            SetPosition(owner.ballOwnDistance);
            owner = null;
        }
        SetVelocityToZero();
    }

    public Dictionary<float, Vector3> GetPredictedPositions(float forSeconds)
    {
        var simulatedTime = 0f;
        var predictions = new Dictionary<float, Vector3>();

        while (forSeconds >= simulatedTime)
        {
            simulatedTime += Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
            predictions.Add(simulatedTime, ballRigidbody.position);
        }

        return predictions;
    }

    private void Dribble()
    {
        SetPosition(owner.ballDribblingDistance);
    }

    private void SetPosition(float forwardMultiplier)
    {
        var ownerForward = owner.transform.forward;
        var footPosition = owner.footTransform.position;

        var ballPosition = footPosition + (ownerForward * forwardMultiplier);
        ballPosition = new Vector3(ballPosition.x, 0.267f, ballPosition.z);
        transform.position = ballPosition;
    }

    private void SetVelocityToZero()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalDetector"))
        {
            HandleGoal();
        }

        if (other.CompareTag("Out"))
        {
            HandleOut();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("GoalPost"))
        {
            ballAudio.PlayGoalPostAudioClip();
        }
    }

    private void HandleGoal()
    {
        if (GameController.Default.finished) return;

        ballAudio.PlayGoalAudioClip();
        GameController.Default.PublishGoal();
    }

    private void HandleOut()
    {
        if (GameController.Default.finished) return;

        ballAudio.PlayOutAudioClip();
        GameController.Default.PublishOut();
    }
}
