using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    private float rotationVelocity;
    private float targetRotationAngle;

    private Ball ball;
    private Animator animator;
    private CharacterController characterController;
    private PlayerStateController playerStateController;

    public Vector3 speed => characterController.velocity;

    [Header("Player")] [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 4;

    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 10;

    [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 1000f)]
    public float rotationSpeed = 200;

    [Tooltip("Acceleration and deceleration")]
    public float speedChangeRate = 10.0f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = 0.01f;

    [Header("Transform")]
    [Tooltip("Right foot transform")]
    public Transform footTransform;

    [Tooltip("Goal transform")]
    public Transform goalTransform;

    [Header("Ball")]
    [Tooltip("When the character has this distance to the ball, the ball can be owned.")]
    public float ballOwnDistance = .5f;

    [Tooltip("Detect ball with a ray cast beamed from this height.")]
    public float ballRangeDetectInHeight = .3f;

    [Tooltip("When the character has this distance to the ball, the ball can be owned.")]
    public float ballDribblingDistance = 1f;

    [Tooltip("When the character has this distance to the goal, shoot the ball.")]
    public float shootingDistance = 20f;

    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
    }

    public void Update()
    {
        GroundedCheck();
        HandlePlayerState();
    }

    public void TriggerWalkToBall()
    {
        Debug.Log($"Triggering walk to ball");
        playerStateController.ChangeState(PlayerState.WalkToBall);
    }

    private void TriggerDribbling()
    {
        Debug.Log("Triggering dribbling");
        ball.Control(this);
        playerStateController.ChangeState(PlayerState.Dribbling);
    }

    private void TriggerShooting()
    {
        Debug.Log("Triggering shooting");
        ball.Uncontrol();
        playerStateController.ChangeState(PlayerState.Shooting);
        animator.SetTrigger("BallKick");
    }

    private void HandlePlayerState()
    {
        var playerState = playerStateController.playerState;

        if (playerState == PlayerState.WalkToBall)
        {
            WalkToBall();
        }

        if (playerState == PlayerState.Dribbling)
        {
            Dribble();
        }
    }

    private void WalkToBall()
    {
        RotateToPosition(ball.transform.position);
        Move(sprintSpeed);
        CheckBallInRange();
    }

    private void Dribble()
    {
        RotateToPosition(goalTransform.position);
        Move(moveSpeed);
        ShootWhenPlayerInShootingArea();
    }

    private void Move(float targetSpeed)
    {
        characterController.Move(transform.forward * (targetSpeed * Time.deltaTime));
        animator.SetInteger("Speed", (int)targetSpeed);
    }

    private void RotateToPosition(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        var targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Quaternion targetRotationQuaternion = Quaternion.Euler(new Vector3(0, targetRotation, 0));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationQuaternion, rotationSpeed * Time.deltaTime);
    }

    private void CheckBallInRange()
    {
        var layerMask = LayerMask.GetMask("Ball");

        var position = transform.position;
        var playerPosition = new Vector3(position.x, ballRangeDetectInHeight, position.z);

        Debug.DrawRay(playerPosition, transform.forward, Color.red);

        if (!Physics.Raycast(playerPosition, transform.forward, out _, ballOwnDistance, layerMask)) return;

        ShootWhenPlayerInShootingArea();

        if (playerStateController.playerState == PlayerState.Shooting) return;

        TriggerDribbling();
    }

    private void ShootWhenPlayerInShootingArea()
    {
        var distanceToGoal = (transform.position - goalTransform.position).magnitude;

        if (distanceToGoal > shootingDistance) return;

        TriggerShooting();
    }

    private void GroundedCheck()
    {
        grounded = transform.position.y <= characterController.skinWidth + groundedOffset;

        if (grounded) return;

        characterController.Move(Physics.gravity * Time.fixedDeltaTime);
    }
}
