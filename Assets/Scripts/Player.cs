using UnityEngine;

enum PlayerState
{
    Idle,
    Walking,
    Shooting,
    WaitingResult
}

public class Player : MonoBehaviour
{
    private const int ShootToGoalAngle = 15;

    [SerializeField] private int movementSpeed = 10;

    private CharacterController characterController;

    private SwipeController swipeController;

    private Vector3? shootTarget;

    private Animator animator;

    private PlayerState playerState = PlayerState.Idle;

    private BallDetector ballDetector;

    private CameraController cameraController;

    private PlayerAudio playerAudio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        swipeController = GetComponent<SwipeController>();
        swipeController.Swiped += SwipeControllerOnSwiped;

        ballDetector = GetComponentInChildren<BallDetector>();
        ballDetector.BallTouched += BallDetectorOnBallTouched;

        cameraController = GetComponent<CameraController>();

        playerAudio = GetComponent<PlayerAudio>();

        BallEventAggregator.Default.Goal += OnGoal;
        BallEventAggregator.Default.Out += OnOut;
    }

    private void OnDestroy()
    {
        swipeController.Swiped -= SwipeControllerOnSwiped;
        ballDetector.BallTouched -= BallDetectorOnBallTouched;

        BallEventAggregator.Default.Goal -= OnGoal;
        BallEventAggregator.Default.Out -= OnOut;
    }

    private void Update()
    {
        if (playerState == PlayerState.Walking)
        {
            MovePlayer();
        }
    }

    private void SwipeControllerOnSwiped(Vector3 target)
    {
        ChangeState(PlayerState.Walking);
        shootTarget = target;
    }

    private void MovePlayer()
    {
        animator.SetInteger("Speed", movementSpeed);

        var move = transform.forward * (Time.deltaTime * movementSpeed);
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

        ChangeState(PlayerState.Shooting);
        animator.SetTrigger("BallKick");
    }

    private void BallDetectorOnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (!part.CompareTag("BallKickDetector")) return;

        playerAudio.PlayBallKickAudio();
        ChangeState(PlayerState.WaitingResult);
        ShootBall(ballGameObject);
    }

    private void ShootBall(GameObject ballGameObject)
    {
        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(ballPosition, shootTarget!.Value, ShootToGoalAngle);

        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(velocity);

        animator.SetInteger("Speed", 0);
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

    private void ChangeState(PlayerState state)
    {
        playerState = state;
    }
}
