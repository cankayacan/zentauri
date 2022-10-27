using System;
using System.Collections;
using UnityEngine;

public class PlayerWalkToBall: MonoBehaviour
{
    private Ball ball;
    private float footHeight;
    
    private CameraController cameraController;
    private PlayerStateController playerStateController;
    private PlayerEffectController playerEffectController;
    private PlayerCharacterController playerCharacterController;

    [Tooltip("The max angle, the player needs to have before moving")]
    [Range(15f, 360f)]
    public float maxAngleBeforeWalking = 15f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 1000f)]
    public float rotationSpeed = 200;

    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 10;

    [Tooltip("When the character has this distance to the ball, the ball can be owned.")]
    public float ballOwnDistance = .5f;

    [Tooltip("Right foot transform")]
    public Transform footTransform;
    
    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cameraController = GetComponent<CameraController>();
        playerStateController = GetComponent<PlayerStateController>();
        playerEffectController = GetComponent<PlayerEffectController>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        
        playerStateController = GetComponent<PlayerStateController>();
        playerStateController.StateChanged += PlayerStateControllerOnStateChanged;
    }

    private void Start()
    {
        footHeight = footTransform.position.y;
    }
    
    public void OnDestroy()
    {
        playerStateController.StateChanged -= PlayerStateControllerOnStateChanged;
    }

    private void PlayerStateControllerOnStateChanged(PlayerState state)
    {
        if (state == PlayerState.WalkToBall)
        {
            cameraController.SwitchCamera(CameraType.Moving);
            playerEffectController.EnableWalkParticles();
            StartCoroutine(nameof(WalkToBall));
        }
    }

    private IEnumerator WalkToBall()
    {
        while (playerStateController.playerState == PlayerState.WalkToBall) 
        {
            var rotationAngleLeft = RotateToBall();
            if (rotationAngleLeft <= maxAngleBeforeWalking)
            {
                playerCharacterController.Move(sprintSpeed);
            }

            CheckBallInRange();

            yield return new WaitForEndOfFrame();
        }
    }
    
    private void CheckBallInRange()
    {
        var layerMask = LayerMask.GetMask("Ball");

        var position = transform.position;
        var playerPosition = new Vector3(position.x, footHeight, position.z);

        Debug.DrawRay(playerPosition, transform.forward, Color.red);

        if (!Physics.Raycast(playerPosition, transform.forward, out RaycastHit hitInfo, ballOwnDistance, layerMask))
        {
            return;
        }

        if (playerStateController.playerState == PlayerState.Shooting) return;
        
        playerStateController.ChangeState(PlayerState.Dribbling);
    }

    private float RotateToBall()
    {
        var targetPosition = ball.transform.position;
        var targetRotation = Utils.GetTargetQuaternion(transform.position, targetPosition);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        return Utils.GetAngleToTurn(transform, targetPosition);
    }
}