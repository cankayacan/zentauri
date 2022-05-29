using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private GameObject trailPrefab;
    
    [SerializeField]
    private GameObject ballGameObject;

    private Ball ball;
    
    private GameObject trailGameObject;

    private TrailRenderer trailRenderer;

    private List<Vector2> path;

    void Awake()
    {
        inputController.StartedTouch += InputControllerOnStartedTouch;
        inputController.StoppedTouch += InputControllerOnStoppedTouch;
        ball = ballGameObject.GetComponent<Ball>();
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
        // trailGameObject.transform.position = trailStartPosition;
        
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
        if (path.Count < 3) return;
        
        var startPosition = path[0];
        var endPosition = path[^1];

        var pathOrderedDesc = path.OrderByDescending(p => p.x).ToList();

        var rightMostPosition = pathOrderedDesc.First();
        var leftMostPosition = pathOrderedDesc.Last();
        
        Debug.Log($"Curve diff {rightMostPosition.x} {startPosition.x}");

        var curveLengthRight = Math.Abs(rightMostPosition.x - startPosition.x);
        var curveLengthLeft = Math.Abs(leftMostPosition.x - startPosition.x);

        var isRightCurve = curveLengthRight > curveLengthLeft;

        var forwardY = isRightCurve ? rightMostPosition.y : leftMostPosition.y;
        var curveLengthVertical = Math.Abs(startPosition.y - forwardY);

        var curveLengthHorizontal = isRightCurve ? curveLengthRight : -1 * curveLengthLeft;
        var curveDirection = new Vector3(curveLengthHorizontal / curveLengthVertical, 0, 1);
        var force = curveDirection * 300;
        
        Debug.Log($"End position {endPosition}");

        var destination = Utils.GetWorldPosition(Camera.main, endPosition);

        if (!destination.HasValue) return;

        Debug.Log($"End world position {destination}");
        ball.Shoot(force, destination.Value);
    }
}
