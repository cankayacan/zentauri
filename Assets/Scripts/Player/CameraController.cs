using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum CameraType
{
    Moving,
    Shooting,
    Finish
}

public class CameraController: MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera movingCamera;
    [SerializeField] private CinemachineVirtualCamera shootingCamera;
    [SerializeField] private CinemachineVirtualCamera finishCamera;

    private Dictionary<CameraType, CinemachineVirtualCamera> cameras;

    public void Awake()
    {
        cameras = new Dictionary<CameraType, CinemachineVirtualCamera>()
        {
            { CameraType.Moving, movingCamera },
            { CameraType.Shooting, shootingCamera },
            { CameraType.Finish, finishCamera },
        };
    }

    public void SwitchCamera(CameraType cameraType)
    {
        foreach (var virtualCamera in cameras)
        {
            virtualCamera.Value.Priority = 0;
        }

        cameras[cameraType].Priority = 1;
    }
}

