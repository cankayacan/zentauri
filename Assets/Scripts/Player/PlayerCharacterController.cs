using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    private float rotationVelocity;
    private float targetRotationAngle;
    private Vector3? positionToWalk;

    private Ball ball;
    private Animator animator;
    private CharacterController characterController;
    private PlayerStateController playerStateController;

    public Vector3 speed => characterController.velocity;

    [Header("Player")] [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 4;

    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 10;

    [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

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

    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
    }

    // public void Start()
    // {
    //     // TESTING TESTING
    //     ball.Shoot(new Vector3(1, 0, 1) * 10);
    //     TriggerWalkToBall();
    // }

    public void Update()
    {
        GroundedCheck();
        Rotate();
        HandlePlayerState();
    }

    public void TriggerWalkToBall()
    {
        positionToWalk = GetPositionToWalk();
        Debug.Log($"Triggering walk to ball {positionToWalk}");

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
            ShootWhenPlayerInShootingArea();
        }
    }

    private void WalkToBall()
    {
        Move(sprintSpeed);
        CheckBallInRange();
    }

    private void Dribble()
    {
        Move(moveSpeed);
    }

    private void Move(float targetSpeed)
    {
        characterController.Move(transform.forward * (targetSpeed * Time.deltaTime));
        animator.SetInteger("Speed", (int)targetSpeed);
    }

    private void Rotate()
    {
        var targetPosition = ball.transform.position;

        if (playerStateController.playerState == PlayerState.Dribbling)
        {
            targetPosition = goalTransform.position;
        }

        if (playerStateController.playerState == PlayerState.WalkToBall && positionToWalk != null)
        {
            targetPosition = positionToWalk.Value;
        }

        var direction = targetPosition - transform.position;
        var targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
            rotationSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private Vector3? GetPositionToWalk()
    {
        var predictedBallPositions = ball.GetPredictedPositions(3f);

        foreach (var entry in predictedBallPositions)
        {
            var distanceToPredictedPosition = entry.Value - transform.position;
            var requiredSpeedToPredictedPosition = distanceToPredictedPosition.magnitude / entry.Key;

            if (requiredSpeedToPredictedPosition < sprintSpeed)
            {
                return entry.Value;
            }
        }

        return null;
    }

    private void CheckBallInRange()
    {
        var layerMask = LayerMask.GetMask("Ball");

        var position = transform.position;
        var playerPosition = new Vector3(position.x, 0.3f, position.z);

        Debug.DrawRay(playerPosition, transform.forward, Color.red);

        if (!Physics.Raycast(playerPosition, transform.forward, out _, .5f, layerMask)) return;

        ShootWhenPlayerInShootingArea();

        if (playerStateController.playerState != PlayerState.Shooting)
        {
            TriggerDribbling();
        }
    }

    private void ShootWhenPlayerInShootingArea()
    {
        var distanceToGoal = (transform.position - goalTransform.position).magnitude;
        Debug.Log($"Distance to goal {distanceToGoal}");

        if (distanceToGoal <= 20)
        {
            TriggerShooting();
        }
    }

    private void GroundedCheck()
    {
        grounded = transform.position.y <= groundedOffset;

        if (!grounded)
        {
            // Debug.Log("NOT GROUNDED!!!");
            characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }
}
