using System;
using UnityEngine;

public static class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane + 0.5f;
        return camera.ScreenToWorldPoint(position);
    }

    public static Vector3? GetWorldPosition(Camera camera, Vector3 position)
    {
        var ray = camera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out var hit))
        {
            return hit.point;
        }

        return null;
    }

    public static Quaternion GetTargetQuaternion(Vector3 currentPosition, Vector3 targetPosition)
    {
        var direction = targetPosition - currentPosition;
        var targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, targetRotation, 0));
    }
    
    public static float GetAngleToTurn(Transform transform, Vector3 targetPosition)
    {
        var targetRotation = Utils.GetTargetQuaternion(transform.position, targetPosition);
        return Math.Abs(transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y);
    }
}

public static class VectorExtensions
{
    public static Vector3 xz(this Vector3 vector)
    {
        var (x, _, z) = vector;
        return new Vector3(x, 0, z);
    }

    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z) =>
        (x, y, z) = (vector.x, vector.y, vector.z);
}

public static class TransformExtensions
{
    public static void Deconstruct(this Transform transform, out Vector3 position, out Quaternion rotation) =>
        (position, rotation) = (transform.position, transform.rotation);
}