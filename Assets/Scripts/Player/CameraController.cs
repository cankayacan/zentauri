using Cinemachine;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera shootingCamera;
    [SerializeField] private CinemachineVirtualCamera goalCamera;

    public void Goal()
    {
        goalCamera.Priority = 1;
        shootingCamera.Priority = 0;
    }
}

