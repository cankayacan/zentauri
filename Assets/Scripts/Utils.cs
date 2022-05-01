using UnityEngine;

public static class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane + 10;
        return camera.ScreenToWorldPoint(position);
    }
}
