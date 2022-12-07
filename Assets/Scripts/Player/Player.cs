using UnityEngine;

[RequireComponent(typeof(PlayerStateController))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerWalkToBall))]
[RequireComponent(typeof(PlayerDribbling))]
[RequireComponent(typeof(PlayerGameEnding))]
public class Player : MonoBehaviour
{
    private PlayerStateController playerStateController;

    private void Awake()
    {
        playerStateController = GetComponent<PlayerStateController>();

        GameController.Default.Goal += OnGoal;
        GameController.Default.Out += OnOut;
    }

    public void Start()
    {
        playerStateController.ChangeState(PlayerState.WalkToBall);
    }

    private void OnDestroy()
    {
        GameController.Default.Goal -= OnGoal;
        GameController.Default.Out -= OnOut;
    }

    private void OnGoal()
    {
        playerStateController.ChangeState(PlayerState.Goal);
    }

    private void OnOut()
    {
        playerStateController.ChangeState(PlayerState.Out);
    }
}