using System;
using System.Collections;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private GameObject trailGameObject;

    TrailRenderer trailRenderer;

    void Awake()
    {
        trailRenderer = trailGameObject.GetComponent<TrailRenderer>();
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
        trailGameObject.transform.position = position;
        trailGameObject.SetActive(true);
        
        trailRenderer.Clear();
        
        StartCoroutine(Trail());
    }

    private void InputControllerOnStoppedTouch(Vector2 position)
    {
        trailGameObject.SetActive(false);
    }
    
    private IEnumerator Trail()
    {
        while (trailGameObject.activeSelf)
        {
            Debug.Log($"loop {inputController.PrimaryPosition}");
            trailGameObject.transform.position = inputController.PrimaryPosition;
            yield return null;
        }
    }
}
