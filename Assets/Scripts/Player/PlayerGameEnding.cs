using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStateController))]
public class PlayerGameEnding: MonoBehaviour
{
    private Ball ball;
    private Animator animator;
    private CameraController cameraController;
    private PlayerStateController playerStateController;

    public void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cameraController = FindObjectOfType<CameraController>();
        
        animator = GetComponent<Animator>();

        playerStateController = GetComponent<PlayerStateController>();
        playerStateController.StateChanged += PlayerStateControllerOnStateChanged;
    }
    
    public void OnDestroy()
    {
        playerStateController.StateChanged -= PlayerStateControllerOnStateChanged;
    }

    private void PlayerStateControllerOnStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Goal:
                Finish(true);
                break;
            case PlayerState.Out:
                Finish(false);
                break;
        }
    }
    
    private void Finish(bool isGoal)
    {
        cameraController.SwitchCamera(CameraType.Finish);
        ball.LeaveControl();
        animator.SetTrigger(isGoal ? "Goal" : "Out");
    }
}
