using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const int ShootToGoalAngle = 15;

    [SerializeField] private int movementSpeed = 10;

    private CharacterController characterController;

    private SwipeController swipeController;

    private Vector3? shootTarget;

    private Animator animator;

    private bool ballInShootingRange;

    private BallDetector ballDetector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        swipeController = GetComponent<SwipeController>();
        swipeController.Swiped += SwipeControllerOnSwiped;

        ballDetector = GetComponentInChildren<BallDetector>();
        ballDetector.BallTouched += BallDetectorOnBallTouched;
        
        BallEventAggregator.Default.Goal += OnGoal;
        BallEventAggregator.Default.Out += OnOut;
    }

    private void Update()
    {
        if (!shootTarget.HasValue) return;
        
        if (ballInShootingRange) return;
        
        animator.SetInteger("Speed", movementSpeed);

        var move = transform.forward * (Time.deltaTime * movementSpeed);
        characterController.Move(move);

        CheckBallInShootingRange();
    }

    private void OnDestroy()
    {
        swipeController.Swiped -= SwipeControllerOnSwiped;
        ballDetector.BallTouched -= BallDetectorOnBallTouched;
    }

    private void SwipeControllerOnSwiped(Vector3 target)
    {
        shootTarget = target;
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
            
        animator.SetTrigger("BallKick");
        ballInShootingRange = true;
    }

    private void BallDetectorOnBallTouched(GameObject part, GameObject ballGameObject)
    {
        if (part.CompareTag("BallKickDetector"))
        {
            ShootBall(ballGameObject);
        }
    }

    private void ShootBall(GameObject ballGameObject)
    {
        var ballPosition = ballGameObject.transform.position;
        var velocity = ProjectileHelper.CalculateVelocity(ballPosition, shootTarget!.Value, ShootToGoalAngle);
    
        var ball = ballGameObject.GetComponent<Ball>();
        ball.Shoot(velocity);

        animator.SetInteger("Speed", 0);
        shootTarget = null;
    }
    
    private void OnGoal()
    {
        animator.SetTrigger("Goal");
    }
    
    private void OnOut()
    {
        animator.SetTrigger("Out");
    }
}
