using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager: MonoBehaviour
{
    private int frameCountToWait = 5;
    private CameraController cameraController;

    public Transform spawnPoint;
    public GameObject trail;
    public GameObject player;
    public Transform goalTransformHost;
    public Transform goalTransformGuest;
    
    public void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        StartCoroutine(nameof(SpawnAfterFewFrames));
    }

    private IEnumerator SpawnAfterFewFrames()
    {
        while (frameCountToWait > 0)
        {
            frameCountToWait--;
            Debug.Log($"Waiting {frameCountToWait}");
            yield return null;
        }
        
        Debug.Log("Spawning");
        
        Spawn();
    }

    private void Spawn()
    {
        var goalTransform = goalTransformHost;

        var playerGameObject = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        var swipeController = playerGameObject.GetComponent<SwipeController>();
        swipeController.trailPrefab = trail;

        var playerDribbling = playerGameObject.GetComponent<PlayerDribbling>();
        playerDribbling.goalTransform = goalTransform;

        cameraController.SetupCameras(playerGameObject, goalTransform);
    }
}
