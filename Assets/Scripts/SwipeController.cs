using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    [SerializeField] private ShootController shootController;

    [SerializeField] private GameObject trailPrefab;

    private GameObject trailGameObject;

    private TrailRenderer trailRenderer;

    private List<Vector2> path;

    void Awake()
    {
        inputController.StartedTouch += InputControllerOnStartedTouch;
        inputController.StoppedTouch += InputControllerOnStoppedTouch;
    }

    private void OnDestroy()
    {
        inputController.StartedTouch -= InputControllerOnStartedTouch;
        inputController.StoppedTouch -= InputControllerOnStoppedTouch;
    }

    private void InputControllerOnStartedTouch(Vector2 position)
    {
        path = new List<Vector2> { position };

        var trailStartPosition = Utils.ScreenToWorld(Camera.main, inputController.PrimaryPosition);
        trailGameObject = Instantiate(trailPrefab, trailStartPosition, Quaternion.identity);

        trailRenderer = trailGameObject.GetComponent<TrailRenderer>();
        trailRenderer.widthMultiplier = 0.05f;

        StartCoroutine(Trail());
    }

    private void InputControllerOnStoppedTouch(Vector2 position)
    {
        path.Add(position);
        Destroy(trailGameObject);
        trailGameObject = null;
        Shoot();
    }

    private IEnumerator Trail()
    {
        var mainCamera = Camera.main;
        while (trailGameObject != null)
        {
            var position = inputController.PrimaryPosition;
            path.Add(position);

            var newTrailPosition = Utils.ScreenToWorld(mainCamera, position);
            trailGameObject.transform.position = newTrailPosition;

            yield return null;
        }
    }

    private void Shoot()
    {
        Debug.Log($"Path count {path.Count}");
        if (path.Count < 3) return;

        var startPosition = path[0];
        var endPosition = path[^1];

        path.RemoveAll(p => p.y > endPosition.y);
        path.RemoveAll(p => p.y < startPosition.y);

        const int checkSlopePerItems = 10;
        var checkPoints = path.Where((_, i) => i % checkSlopePerItems == 0).ToList();

        if (checkPoints.Count < 2) return;

        var furthestPoint = checkPoints
            .OrderByDescending(x => HandleUtility.DistancePointToLine(x, startPosition, endPosition)).First();
        
        var curveDirection = (furthestPoint - startPosition).normalized;
        Debug.Log($"Curve direction {curveDirection}");

        var target = Utils.GetWorldPosition(Camera.main, endPosition);

        if (!target.HasValue) return;

        shootController.Fire(target.Value);
    }
}
