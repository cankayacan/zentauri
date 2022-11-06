using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    private Vector3? lastPosition;
    private Animator animator;
    private CharacterController characterController;
    private PlayerStateController playerStateController;

    public Vector3 speed;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = 0.01f;

    [Tooltip("When the character has this distance to the ball, the ball can be owned.")]
    public float ballDribblingDistance = 0.8f;

    [Tooltip("Right foot transform")]
    public Transform footTransform;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        playerStateController = GetComponent<PlayerStateController>();
    }

    public void Update()
    {
        CheckGrounded();
        CheckOnTopOfBall();
        CalculateSpeed();
    }

    public void Move(float targetSpeed)
    {
        characterController.Move(transform.forward * (targetSpeed * Time.deltaTime));
    }

    private void CheckGrounded()
    {
        grounded = transform.position.y <= characterController.skinWidth + groundedOffset;

        if (grounded) return;

        characterController.Move(Physics.gravity * Time.fixedDeltaTime);
    }

    private void CheckOnTopOfBall()
    {
        var layerMask = LayerMask.GetMask("Ball");

        var playerPosition = transform.position;

        Debug.DrawRay(playerPosition, transform.up * -1, Color.blue);

        if (Physics.Raycast(playerPosition, transform.up * -1, out RaycastHit hitInfo, Int32.MaxValue, layerMask))
        {
            playerStateController.ChangeState(PlayerState.Dribbling);
            Debug.Log($"under ball {hitInfo.collider.name}");
        }
    }

    private void CalculateSpeed()
    {
        var position = transform.position;

        if (lastPosition.HasValue)
        {
            speed = (position - lastPosition.Value) / Time.deltaTime;
            animator.SetFloat("Speed", speed.magnitude);
        }

        lastPosition = position;
    }
}
