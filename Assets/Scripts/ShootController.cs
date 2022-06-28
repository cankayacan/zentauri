using UnityEngine;

public class ShootController : MonoBehaviour
{
    private const int ShootToGoalAngle = 15;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject ballPrefab;

    public void Fire(Vector3 target)
    {
        var position = firePoint.position;
        var ballGameObject = Instantiate(ballPrefab, position, Quaternion.identity);
        var velocity = GetVelocity(position, target, ShootToGoalAngle);
        ballGameObject.GetComponent<Ball>().Shoot(velocity); 
    }

    static Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        var gravity = Physics.gravity.magnitude;
        var angle = initialAngle * Mathf.Deg2Rad;

        var planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        var planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        var distance = Vector3.Distance(planarTarget, planarPosition);
        var yOffset = currentPos.y - targetPos.y;

        var initialVelocity = (1 / Mathf.Cos(angle)) *
                              Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) /
                                         (distance * Mathf.Tan(angle) + yOffset));

        var velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        var angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) *
                                  (targetPos.x > currentPos.x ? 1 : -1);
        var finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
