using System;
using System.Collections;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private GameObject trailPrefab;
    
    private GameObject trailGameObject;

    private TrailRenderer trailRenderer;

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
        trailGameObject = Instantiate(trailPrefab, position, Quaternion.identity);
        trailGameObject.transform.position = inputController.PrimaryPosition;
        trailRenderer = trailGameObject.GetComponent<TrailRenderer>();
        trailRenderer.widthMultiplier = 0.05f;
        StartCoroutine(Trail());
    }

    private void InputControllerOnStoppedTouch(Vector2 position)
    {
        Destroy(trailGameObject);
        trailGameObject = null;
    }
    
    private IEnumerator Trail()
    {
        while (trailGameObject != null)
        {
            trailGameObject.transform.position = inputController.PrimaryPosition;
            yield return null;
        }
    }
}
