using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerInput playerInput;

    public event Action<Vector2> StartedTouch;
    
    public event Action<Vector2> StoppedTouch;

    public Vector3 PrimaryPosition => Utils.ScreenToWorld(Camera.main, playerInput.actions["PrimaryPosition"].ReadValue<Vector2>());

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Input controller");
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["PrimaryContact"].started += StartTouchPrimary;
        playerInput.actions["PrimaryContact"].canceled += StopTouchPrimary;
    }

    private void OnDestroy()
    {
        playerInput.actions["PrimaryContact"].started -= StartTouchPrimary;
        playerInput.actions["PrimaryContact"].canceled -= StopTouchPrimary;
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        Debug.Log("Started touch primary");
        StartedTouch?.Invoke(PrimaryPosition);
    }
    
    private void StopTouchPrimary(InputAction.CallbackContext ctx)
    {
        Debug.Log("Stopped touch primary");
        StoppedTouch?.Invoke(PrimaryPosition);
    }
}
