using UnityEngine;

public enum PlayerState
{
    Idle,
    WalkToBall,
    Dribbling,
    Shooting,
    WaitingShootResult,
    Goal,
    Out
}

public class PlayerStateController: MonoBehaviour
{
    public PlayerState playerState = PlayerState.Idle;

    public void ChangeState(PlayerState state)
    {
        Debug.Log($"Chaging state to {state}");
        playerState = state;
    }
}
