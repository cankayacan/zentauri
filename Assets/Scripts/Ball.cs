using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (BallAudio))]
public class Ball : MonoBehaviour
{
    private BallAudio ballAudio;
    private Rigidbody ballRigidbody;
    private float ballGroundedHeight;
    private List<ParticleSystem> particles;
    private bool collidedAfterShooting;
    private bool showingParticles;
    private Vector3 speed => ballRigidbody.velocity;
    private PlayerCharacterController owner;

    public float showParticlesSpeed = 8;
    public float dribblingDistance = 0.75f;
    public float angularVelocityMultiplier = 1.5f;
    public float maxCurveForceVelocity = 30f;

    private float curveForce;

    public void Awake()
    {
        ballAudio = GetComponent<BallAudio>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballGroundedHeight = transform.position.y;
        particles = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void Update()
    {
        UpdateParticles();

        if (owner == null) return;

        Dribble();
        UpdateAngularVelocity();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalForce();
    }

    public void Shoot(Vector3 target, Vector3 velocity, float curveAngle)
    {
        var horizontalVelocityToApply = ScaleAngleToCurveVelocity(curveAngle);
        Debug.Log($"curveAngle {curveAngle} horizontalVelocityToApply {horizontalVelocityToApply}");
        var distanceX = target.x - transform.position.x;
        var estimatedShootingTime = distanceX / velocity.x;

        // V * t = 1/2 * g * t^2 -> g = V * t / 2
        curveForce = 2 * horizontalVelocityToApply / estimatedShootingTime;
        Debug.Log($"Distance {distanceX} Time {estimatedShootingTime} Curve {curveForce}");

        var initialVelocityZ = velocity.z + horizontalVelocityToApply;
        ballRigidbody.velocity = new Vector3(velocity.x, velocity.y, initialVelocityZ);
        collidedAfterShooting = false;
    }

    public void Control(PlayerCharacterController characterController)
    {
        if (owner == characterController) return;

        SetVelocityToZero();
        owner = characterController;

        IgnoreCollisions(true);
    }

    public void LeaveControl()
    {
        IgnoreCollisions(false);
        owner = null;
    }

    public void PrepareShooting()
    {
        if (owner == null) return;

        ballRigidbody.velocity = owner.transform.forward * -2;
        IgnoreCollisions(false);
        owner = null;
    }

    private void Dribble()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        var ownerTransform = owner.transform;
        var ownerForward = ownerTransform.forward;
        var footPosition = ownerTransform.position;

        var ballPosition = footPosition + (ownerForward * dribblingDistance);
        ballPosition = new Vector3(ballPosition.x, ballGroundedHeight, ballPosition.z);
        transform.position = ballPosition;
    }

    public void SetVelocityToZero()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalDetector"))
        {
            HandleGoal();
        }

        if (other.CompareTag("Out"))
        {
            HandleOut();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("GoalPost"))
        {
            ballAudio.PlayGoalPostAudioClip();
        }

        var collidedWithGroundOrPlayer = other.CompareTag("Ground") || other.CompareTag("Player");
        if (!collidedWithGroundOrPlayer)
        {
            collidedAfterShooting = true;
        }
    }

    private void HandleGoal()
    {
        if (GameController.Default.finished) return;

        ballAudio.PlayGoalAudioClip();
        GameController.Default.PublishGoal();
    }

    private void HandleOut()
    {
        if (GameController.Default.finished) return;

        ballAudio.PlayOutAudioClip();
        GameController.Default.PublishOut();
    }

    private void IgnoreCollisions(bool ignore)
    {
        if (owner == null) return;

        Physics.IgnoreCollision(owner.GetComponent<Collider>(), GetComponent<Collider>(), ignore);
    }

    private void UpdateAngularVelocity()
    {
        ballRigidbody.angularVelocity = owner.transform.right * (owner.speed.magnitude * angularVelocityMultiplier);
    }

    private void UpdateParticles()
    {
        var speedMagnitude = speed.magnitude;

        var showParticles = speedMagnitude > showParticlesSpeed;

        if (collidedAfterShooting) showParticles = false;

        if (showingParticles == showParticles) return;

        showingParticles = showParticles;

        if (showParticles)
        {
            particles.ForEach(p => p.Play());
        }
        else
        {
            particles.ForEach(p => p.Stop());
        }
    }

    private float ScaleAngleToCurveVelocity(float angle)
    {
        const float minAngle = -90f;
        const float maxAngle = 90f;
        var angleConstrained = (angle < minAngle) ? minAngle : (angle > maxAngle) ? maxAngle : angle;
        return angleConstrained / maxAngle * maxCurveForceVelocity;
    }

    private void ApplyHorizontalForce()
    {
        if (collidedAfterShooting) return;

        ballRigidbody.AddForce(-1 * new Vector3(0, 0, curveForce * Time.deltaTime), ForceMode.VelocityChange);
    }
}
