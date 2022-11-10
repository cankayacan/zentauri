using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStateController))]
[RequireComponent(typeof(PlayerEffectController))]
[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerWalkToBall : MonoBehaviour
{
    private Ball ball;
    private CameraController cameraController;

    private BallDetector ballInRangeDetector;
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
    public float sprintSpeed = 12;

    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cameraController = FindObjectOfType<CameraController>();
        
        ballInRangeDetector = GameObject.FindWithTag("BallInRangeDetector").GetComponent<BallDetector>();
        ballInRangeDetector.BallTouched += OnBallInRange;

        playerEffectController = GetComponent<PlayerEffectController>();
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

            yield return new WaitForEndOfFrame();
        }
    }

    private float RotateToBall()
    {
        var targetPosition = ball.transform.position;
        var targetRotation = Utils.GetTargetQuaternion(transform.position, targetPosition);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        return Utils.GetAngleToTurn(transform, targetPosition);
    }

    private void OnBallInRange(GameObject part, GameObject ballGameObject)
    {
        if (playerStateController.playerState == PlayerState.Shooting) return;
        
        playerStateController.ChangeState(PlayerState.Dribbling);
    }
}