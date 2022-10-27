using System;
using System.Collections;
using UnityEngine;

public class PlayerDribbling: MonoBehaviour
{
    private Ball ball;
    private float footHeight;

    private PlayerStateController playerStateController;
    private PlayerCharacterController playerCharacterController;

    [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 4;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 1000f)]
    public float rotationSpeed = 200;

    [Tooltip("The max angle, the player needs to have  before shooting")]
    public float maxAngleBeforeStopMotion = 5f;

    [Tooltip("Goal transform")]
    public Transform goalTransform;
    
    [Tooltip("When the character has this distance to the goal, shoot the ball.")]
    public float shootingDistance = 20f;
    
    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        playerStateController = GetComponent<PlayerStateController>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        
        playerStateController = GetComponent<PlayerStateController>();
        playerStateController.StateChanged += PlayerStateControllerOnStateChanged;
    }
    
    public void OnDestroy()
    {
        playerStateController.StateChanged -= PlayerStateControllerOnStateChanged;
    }

    private void PlayerStateControllerOnStateChanged(PlayerState state)
    {
        if (state == PlayerState.Dribbling)
        {
            ball.Control(playerCharacterController);
            StartCoroutine(nameof(Dribble));
        }
    }
    
    private IEnumerator Dribble()
    {
        while (playerStateController.playerState == PlayerState.Dribbling)
        {
            RotateToGoal();
            playerCharacterController.Move(moveSpeed);
            StopMotionIfPlayerInShootingArea();
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private void RotateToGoal()
    {
        var targetRotation = Utils.GetTargetQuaternion(transform.position, goalTransform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void StopMotionIfPlayerInShootingArea()
    {
        var directionToGoal = goalTransform.position - transform.position;

        var distanceToGoal = (directionToGoal).magnitude;

        if (distanceToGoal > shootingDistance) return;

        var headingGoal = Vector3.Dot(transform.forward.normalized, directionToGoal.normalized) > 0.8;

        if (!headingGoal) return;

        var goalAngle = Utils.GetAngleToTurn(transform, goalTransform.position);

        if (goalAngle > maxAngleBeforeStopMotion) return;

        playerStateController.ChangeState(PlayerState.Shooting);
    }
}