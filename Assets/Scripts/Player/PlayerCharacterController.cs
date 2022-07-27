using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    private bool sprint = false;

    private Ball ball;
    private Animator animator;
    private CharacterController characterController;
    private PlayerStateController playerStateController;

    [Header("Player")] [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 10;

    [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = 0.01f;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        GroundedCheck();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!Grounded)
        {
            characterController.Move(Physics.gravity * Time.deltaTime);
        }

        if (playerStateController.playerState != PlayerState.FreeToWalk) return;

        var targetSpeed = sprint ? SprintSpeed : MoveSpeed;

        animator.SetInteger("Speed", (int)targetSpeed);

        var move = transform.forward * (Time.deltaTime * targetSpeed);
        characterController.Move(move);

        CheckBallInShootingRange();
    }

    private void CheckBallInShootingRange()
    {
        var layerMask = LayerMask.GetMask("Ball");

        var position = transform.position;
        var playerPosition = new Vector3(position.x, 0.3f, position.z);

        Debug.DrawRay(playerPosition, transform.forward, Color.red);

        if (!Physics.Raycast(playerPosition, transform.forward, out var hit,
                Mathf.Infinity,
                layerMask)) return;

        var distance = (hit.point - transform.position).magnitude;

        if (!(distance < 0.6f)) return;

        playerStateController.ChangeState(PlayerState.Shooting);
        animator.SetTrigger("BallKick");
    }

    private void GroundedCheck()
    {
        Grounded = transform.position.y <= GroundedOffset;
    }
}
