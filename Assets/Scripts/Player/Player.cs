using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof (PlayerCharacterController), typeof (PlayerStateController))]
[RequireComponent(typeof (CameraController), typeof (SwipeController))]
[RequireComponent(typeof (PlayerAudio), typeof (Animator))]
[RequireComponent(typeof (PlayerInput), typeof (PlayerEffectController))]
public class Player : MonoBehaviour
{
    [SerializeField] private int shootToGoalAngle = 15;

    private Vector3? shootTarget;

    private PlayerCharacterController playerCharacterController;
    private PlayerStateController playerStateController;
    private CameraController cameraController;
    private SwipeController swipeController;
    private BallDetector ballDetector;
    private PlayerAudio playerAudio;
    private Animator animator;

    private void Awake()
    {
        playerCharacterController = GetComponent<PlayerCharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
        animator = GetComponent<Animator>();

        swipeController = GetComponent<SwipeController>();
        swipeController.Swiped += SwipeControllerOnSwiped;

        ballDetector = GetComponentInChildren<BallDetector>();
        ballDetector.BallTouched += BallDetectorOnBallTouched;

        cameraController = GetComponent<CameraController>();

        playerAudio = GetComponent<PlayerAudio>();

        GameController.Default.Goal += OnGoal;
        GameController.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        swipeController.Swiped -= SwipeControllerOnSwiped;
        ballDetector.BallTouched -= BallDetectorOnBallTouched;

        GameController.Default.Goal -= OnGoal;
        GameController.Default.Out -= OnOut;
    }

    private void SwipeControllerOnSwiped(Vector3 target)
    {
        if (GameController.Default.finished) return;

        playerCharacterController.HandleSwipe();
        shootTarget = target;
    }

    private void BallDetectorOnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (!part.CompareTag("BallKickDetector")) return;

        if (playerStateController.playerState != PlayerState.Shooting) return;

        playerAudio.PlayBallKickAudio();
        playerStateController.ChangeState(PlayerState.WaitingShootResult);
        ShootBall(ballGameObject);
    }

    private void ShootBall(GameObject ballGameObject)
    {
        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(ballPosition, shootTarget!.Value, shootToGoalAngle);

        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(velocity);

        Invoke("AfterShootBall", 2f);

        animator.SetInteger("Speed", 0);
    }

    private void AfterShootBall()
    {
        if (GameController.Default.finished) return;

        playerCharacterController.TriggerWalkToBall();
        cameraController.SwitchCamera(CameraType.Moving);
    }

    private void OnGoal()
    {
        playerStateController.ChangeState(PlayerState.Goal);
        cameraController.SwitchCamera(CameraType.Finish);
    }

    private void OnOut()
    {
        playerStateController.ChangeState(PlayerState.Out);
        cameraController.SwitchCamera(CameraType.Finish);
    }
}
