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
}
