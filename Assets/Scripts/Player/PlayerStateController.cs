using UnityEngine;

public enum PlayerState
{
    Idle,
    FreeToWalk,
    Shooting,
    WaitingResult
}

public class PlayerStateController: MonoBehaviour
{
    public PlayerState playerState = PlayerState.Idle;

    public void ChangeState(PlayerState state)
    {
        playerState = state;
    }
}
