using UnityEngine;

[RequireComponent(typeof (PlayerCharacterController), typeof (PlayerStateController))]
[RequireComponent(typeof (CameraController), typeof (SwipeController))]
[RequireComponent(typeof (PlayerAudio), typeof (Animator))]
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
        playerCharacterController.TriggerWalkToBall();
        shootTarget = target;
    }

    private void BallDetectorOnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (!part.CompareTag("BallKickDetector")) return;

        if (playerStateController.playerState != PlayerState.Shooting) return;

        playerAudio.PlayBallKickAudio();
        playerStateController.ChangeState(PlayerState.WaitingResult);
        ShootBall(ballGameObject);
    }

    private void ShootBall(GameObject ballGameObject)
    {
        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(ballPosition, shootTarget!.Value, shootToGoalAngle);

        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(velocity);

        Invoke("AfterShootBall", 3f);

        animator.SetInteger("Speed", 0);
    }

    private void AfterShootBall()
    {
        if (GameController.Default.finished) return;

        playerCharacterController.TriggerWalkToBall();
    }

    private void OnGoal()
    {
        animator.SetTrigger("Goal");
        cameraController.Goal();
    }

    private void OnOut()
    {
        animator.SetTrigger("Out");
    }
}
