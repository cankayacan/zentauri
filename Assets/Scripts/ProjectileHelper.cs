using System;
using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    private const bool UseLowAngle = true;

    private static void LaunchAngle(float speed, float distance, float yOffset, float gravity, out float angle0,
        out float angle1)
    {
        Debug.Log($"Speed{speed} distance{distance} yOffset{yOffset}");
        angle0 = angle1 = 0;

        var speedSquared = speed * speed;

        var operandA = Mathf.Pow(speed, 4);
        var operandB = gravity * (gravity * (distance * distance) + (2 * Mathf.Abs(yOffset) * speedSquared));

        // Target is not in range
        if (operandB > operandA)
        {
            return;
        }

        var root = Mathf.Sqrt(operandA - operandB);

        angle0 = Mathf.Atan((speedSquared + root) / (gravity * distance));
        angle1 = Mathf.Atan((speedSquared - root) / (gravity * distance));
    }

    public static Vector3 CalculateVelocity(float speed, Vector3 currentPos, Vector3 targetPos)
    {
        var gravity = Physics.gravity.magnitude;

        var planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        var planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        var distance = Vector3.Distance(planarTarget, planarPosition);
        var yOffset = currentPos.y - targetPos.y;

        LaunchAngle(speed, distance, yOffset, gravity, out var angle0, out var angle1);

        var angle = UseLowAngle ? angle1 : angle0;

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
