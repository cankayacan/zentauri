using System;
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

    public event Action<PlayerState> StateChanged;

    public void ChangeState(PlayerState state)
    {
        if (playerState == state) return;

        Debug.Log($"Chaging state to {state}");

        playerState = state;
        StateChanged?.Invoke(state);
    }
}
