using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum CameraType
{
    None,
    Moving,
    Shooting,
    Finish
}

public class CameraController: MonoBehaviour
{
    private Dictionary<CameraType, CinemachineVirtualCamera> cameras;
    
    [SerializeField] private CinemachineVirtualCamera movingCamera;
    [SerializeField] private CinemachineVirtualCamera shootingCamera;
    [SerializeField] private CinemachineVirtualCamera finishCamera;

    public void Awake()
    {
        cameras = new Dictionary<CameraType, CinemachineVirtualCamera>
        {
            { CameraType.Moving, movingCamera },
            { CameraType.Shooting, shootingCamera },
            { CameraType.Finish, finishCamera },
        };
    }

    public void SwitchCamera(CameraType cameraType)
    {
        if (cameras[cameraType].Priority == 1) return;

        foreach (var virtualCamera in cameras)
        {
            virtualCamera.Value.Priority = 0;
        }

        cameras[cameraType].Priority = 1;
    }
    
    public void SetupCameras(GameObject playerGameObject, Transform goalTransform)
    {
        var playerTransform = playerGameObject.transform;
        
        movingCamera.LookAt = playerTransform;
        movingCamera.Follow = playerTransform;

        shootingCamera.Follow = playerTransform;
        shootingCamera.LookAt = goalTransform;

        finishCamera.Follow = playerTransform;
        finishCamera.LookAt = playerTransform;
    }
}

