using System;
using UnityEngine;
using Cinemachine;

public enum LockDirection
{
    X,
    Y,
    Z
}

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's in the specified co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraLock : CinemachineExtension
{
    [Tooltip("Lock the camera's direction to this game object")] [SerializeField]
    public GameObject lockObject;

    [Tooltip("Lock the camera in this direction")] [SerializeField]
    public LockDirection lockDirection;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var position = state.RawPosition;
            position = GetLockedPosition(position);
            state.RawPosition = position;
        }
    }

    private Vector3 GetLockedPosition(Vector3 position)
    {
        switch (lockDirection)
        {
            case LockDirection.X:
                position.x = lockObject.transform.position.x;
                break;
            case LockDirection.Y:
                position.y = lockObject.transform.position.y;
                break;
            case LockDirection.Z:
                position.z = lockObject.transform.position.z;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return position;
    }
}
