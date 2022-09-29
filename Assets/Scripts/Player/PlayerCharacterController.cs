using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    private float targetRotationAngle;
    private bool stopMotion;

    private Vector3? lastPosition;
    private Ball ball;
    private Animator animator;
    private CharacterController characterController;
    private PlayerStateController playerStateController;
    private CameraController cameraController;
    private PlayerEffectController playerEffectController;

    public Vector3 speed;

    [Header("Player")] [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 4;

    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 10;

    [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 1000f)]
    public float rotationSpeed = 200;

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

    [Tooltip("When the character has this distance to the ball, the ball can be owned.")]
    public float ballDribblingDistance = 1f;

    [Tooltip("When the character has this distance to the goal, shoot the ball.")]
    public float shootingDistance = 20f;

    public void Start()
    {
        TriggerWalkToBall();
    }

    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
        cameraController = GetComponent<CameraController>();
        playerEffectController = GetComponent<PlayerEffectController>();
    }

    public void Update()
    {
        GroundedCheck();
        HandlePlayerState();
        CalculateSpeed();
    }

    public void TriggerWalkToBall()
    {
        playerStateController.ChangeState(PlayerState.WalkToBall);
        playerEffectController.EnableWalkParticles();
    }

    private void TriggerShooting()
    {
        playerStateController.ChangeState(PlayerState.Shooting);
        animator.SetTrigger("BallKick");
        ball.SetVelocityToZero();
        cameraController.SwitchCamera(CameraType.Shooting);
        playerEffectController.DisableWalkParticles();
    }

    public void WaitSwipe()
    {
        animator.enabled = false;
    }

    public void HandleSwipe()
    {
        animator.enabled = true;
        ball.PrepareShooting();
        playerEffectController.EnableShootingParticles();
    }

    private void TriggerDribbling()
    {
        ball.Control(this);
        playerStateController.ChangeState(PlayerState.Dribbling);
    }

    private void HandlePlayerState()
    {
        var playerState = playerStateController.playerState;

        switch (playerState)
        {
            case PlayerState.Shooting:
                break;
            case PlayerState.Goal:
                Finish(true);
                break;
            case PlayerState.Out:
                Finish(false);
                break;
            case PlayerState.WalkToBall:
                WalkToBall();
                break;
            case PlayerState.Dribbling:
                Dribble();
                break;
        }
    }

    private void Finish(bool isGoal)
    {
        Debug.Log("Finishing");
        ball.LeaveControl();
        animator.SetTrigger(isGoal ? "Goal" : "Out");
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
        StopMotionIfPlayerInShootingArea();
    }

    private void Move(float targetSpeed)
    {
        characterController.Move(transform.forward * (targetSpeed * Time.deltaTime));
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
        var playerPosition = new Vector3(position.x, footTransform.position.y, position.z);

        Debug.DrawRay(playerPosition, transform.forward, Color.red);

        if (!Physics.Raycast(playerPosition, transform.forward, out RaycastHit hitInfo, ballOwnDistance, layerMask))
        {
            return;
        }

        Debug.Log($"Ball in range {hitInfo.collider.gameObject.name}");

        StopMotionIfPlayerInShootingArea();

        if (playerStateController.playerState == PlayerState.Shooting) return;

        TriggerDribbling();
    }

    private void StopMotionIfPlayerInShootingArea()
    {
        var directionToGoal = goalTransform.position - transform.position;

        var distanceToGoal = (directionToGoal).magnitude;

        if (distanceToGoal > shootingDistance) return;

        var headingGoal = Vector3.Dot(transform.forward.normalized, directionToGoal.normalized) > 0.8;

        if (!headingGoal) return;

        TriggerShooting();
    }

    private void GroundedCheck()
    {
        grounded = transform.position.y <= characterController.skinWidth + groundedOffset;

        if (grounded) return;

        characterController.Move(Physics.gravity * Time.fixedDeltaTime);
    }

    private void CalculateSpeed()
    {
        var position = transform.position;

        if (lastPosition.HasValue)
        {
            speed = (position - lastPosition.Value) / Time.deltaTime;
            animator.SetInteger("Speed", (int)speed.magnitude);
        }

        lastPosition = position;
    }
}
