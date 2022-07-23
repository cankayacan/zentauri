using System;
using Cinemachine;
using UnityEngine;

public class CameraController: Singleton<CameraController>
{
    [SerializeField] private CinemachineVirtualCamera shootingCamera;
    [SerializeField] private CinemachineVirtualCamera goalCamera;

    private bool goal;

    private void Update()
    {
        if (goal)
        {
            MoveGoalCamera();
        }
    }
    
    public void Goal()
    {
        goalCamera.Priority = 1;
        shootingCamera.Priority = 0;
        
        goal = true;
    }

    private void MoveGoalCamera()
    {
        goalCamera.transform.Translate(Vector3.right * Time.deltaTime);
    }
}

