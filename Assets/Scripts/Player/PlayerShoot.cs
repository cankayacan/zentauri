using UnityEngine;

[RequireComponent(typeof (PlayerEffectController))]
[RequireComponent(typeof (CameraController))]
[RequireComponent(typeof (SwipeController))]
[RequireComponent(typeof (PlayerAudio))]
[RequireComponent(typeof (Animator))]
public class PlayerShoot: MonoBehaviour
{
    private Vector3? shootTarget;

    private PlayerEffectController playerEffectController;
    private PlayerStateController playerStateController;
    private CameraController cameraController;
    private SwipeController swipeController;
    private BallDetector ballDetector;
    private PlayerAudio playerAudio;
    private Animator animator;
    private Ball ball;

    public float shootSpeed = 30f;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>();

        playerEffectController = GetComponent<PlayerEffectController>();
        cameraController = GetComponent<CameraController>();
        playerAudio = GetComponent<PlayerAudio>();
        animator = GetComponent<Animator>();

        playerStateController = GetComponent<PlayerStateController>();
        playerStateController.StateChanged += PlayerStateControllerOnStateChanged;

        swipeController = GetComponent<SwipeController>();
        swipeController.Swiped += SwipeControllerOnSwiped;

        ballDetector = GetComponentInChildren<BallDetector>();
        ballDetector.BallTouched += BallDetectorOnBallTouched;
    }

    private void OnDestroy()
    {
        ballDetector.BallTouched -= BallDetectorOnBallTouched;
        swipeController.Swiped -= SwipeControllerOnSwiped;
    }
    
    private void PlayerStateControllerOnStateChanged(PlayerState state)
    {
        if (state == PlayerState.Shooting)
        {
            TriggerShooting();
        }
    }

    private void TriggerShooting()
    {
        cameraController.SwitchCamera(CameraType.Shooting);
        animator.SetTrigger("BallKick");
        ball.SetVelocityToZero();
        playerEffectController.DisableWalkParticles();
    }
    
    private void WaitSwipe()
    {
        animator.enabled = false;
        Debug.Log("animator disabled");
    }
    
    private void SwipeControllerOnSwiped(Vector3 target)
    {
        if (GameController.Default.finished) return;

        HandleSwipe();
        shootTarget = target;
    }

    private void HandleSwipe()
    {
        animator.enabled = true;
        ball.PrepareShooting();
        playerEffectController.EnableShootingParticles();
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
        if (!shootTarget.HasValue) return;

        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(shootSpeed, ballPosition, shootTarget!.Value);

        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(velocity);

        Invoke("AfterShootBall", 2f);

        animator.SetFloat("Speed", 0f);

        shootTarget = null;
    }
    
    private void AfterShootBall()
    {
        if (GameController.Default.finished) return;

        playerStateController.ChangeState(PlayerState.WalkToBall);
    }
}
