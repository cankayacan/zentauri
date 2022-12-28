using UnityEngine;

[RequireComponent(typeof (PlayerEffectController))]
[RequireComponent(typeof (SwipeController))]
[RequireComponent(typeof (PlayerAudio))]
[RequireComponent(typeof (Animator))]
public class PlayerShoot: MonoBehaviour
{
    private Vector3? shootTarget;
    private float curveAngle;

    private PlayerEffectController playerEffectController;
    private PlayerStateController playerStateController;
    private CameraController cameraController;
    private SwipeController swipeController;
    private BallDetector ballKickDetector;
    private PlayerAudio playerAudio;
    private Animator animator;
    private Ball ball;

    public float shootSpeed = 30f;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cameraController = FindObjectOfType<CameraController>();

        playerEffectController = GetComponent<PlayerEffectController>();
        playerAudio = GetComponent<PlayerAudio>();
        animator = GetComponent<Animator>();

        playerStateController = GetComponent<PlayerStateController>();
        playerStateController.StateChanged += PlayerStateControllerOnStateChanged;

        swipeController = GetComponent<SwipeController>();
        swipeController.Swiped += SwipeControllerOnSwiped;

        ballKickDetector = GameObject.FindWithTag("BallKickDetector").GetComponent<BallDetector>();
        ballKickDetector.BallTouched += OnBallTouched;
    }

    private void OnDestroy()
    {
        ballKickDetector.BallTouched -= OnBallTouched;
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

    private void SwipeControllerOnSwiped(Vector2 target, float curveAngle)
    {
        if (GameController.Default.finished) return;
        
        shootTarget = Utils.GetWorldPosition(Camera.main, target);
        this.curveAngle = curveAngle;

        if (!shootTarget.HasValue) return;

        HandleSwipe();
    }

    private void HandleSwipe()
    {
        animator.enabled = true;
        ball.PrepareShooting();
        playerEffectController.EnableShootingParticles();
    }

    private void OnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (playerStateController.playerState != PlayerState.Shooting) return;

        playerAudio.PlayBallKickAudio();
        playerStateController.ChangeState(PlayerState.WaitingShootResult);
        ShootBall(ballGameObject);
    }

    private void ShootBall(GameObject ballGameObject)
    {
        if (!shootTarget.HasValue) return;

        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(shootSpeed, ballPosition, shootTarget.Value);

        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(shootTarget.Value, velocity, curveAngle);

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
