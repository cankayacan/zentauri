using UnityEngine;

[RequireComponent(typeof (BallAudio))]
public class Ball : MonoBehaviour
{
    private BallAudio ballAudio;
    private Rigidbody ballRigidbody;
    private float ballGroundedHeight;
    public PlayerCharacterController owner;

    public Vector3 speed => ballRigidbody.velocity;

    public void Awake()
    {
        ballAudio = GetComponent<BallAudio>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballGroundedHeight = transform.position.y;
    }

    public void Update()
    {
        if (owner == null) return;

        Dribble();

        ballRigidbody.angularVelocity = owner.transform.right * owner.speed.magnitude;
    }

    public void Shoot(Vector3 velocity)
    {
        ballRigidbody.velocity = velocity;
    }

    public void Control(PlayerCharacterController characterController)
    {
        if (owner == characterController) return;

        SetVelocityToZero();
        owner = characterController;
    }

    public void LeaveControl()
    {
        owner = null;
    }

    public void PrepareShooting()
    {
        SetVelocityToZero();
        ballRigidbody.velocity = owner.transform.forward * -2;
        owner = null;
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
        ballPosition = new Vector3(ballPosition.x, ballGroundedHeight, ballPosition.z);
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
