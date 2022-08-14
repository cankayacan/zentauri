using Cinemachine;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera shootingCamera;
    [SerializeField] private CinemachineVirtualCamera goalCamera;

    private void Update()
    {
        if (goalCamera.Priority == 1)
        {
            MoveGoalCamera();
        }
    }

    public void Goal()
    {
        goalCamera.Priority = 1;
        shootingCamera.Priority = 0;
    }

    private void MoveGoalCamera()
    {
        goalCamera.transform.Translate(Vector3.right * Time.deltaTime);
    }
}

