using UnityEngine;

public enum PlayerState
{
    Idle,
    WalkToBall,
    Dribbling,
    Shooting,
    WaitingResult
}

public class PlayerStateController: MonoBehaviour
{
    public PlayerState playerState = PlayerState.Idle;

    public void ChangeState(PlayerState state)
    {
        Debug.Log($"Changing state {state}");
        playerState = state;
    }
}
