using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    public static Vector3 CalculateVelocity(Vector3 currentPos, Vector3 targetPos)
    {
        var gravity = Physics.gravity.magnitude;

        var planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        var planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        var distance = Vector3.Distance(planarTarget, planarPosition);
        var yOffset = currentPos.y - targetPos.y;

        var angle = Mathf.Atan2(Mathf.Abs(yOffset) * 1.5f, distance);

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
