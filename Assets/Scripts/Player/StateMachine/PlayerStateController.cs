using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerStates CurrentState { get; private set; }

    public void SetState (PlayerStates newState)
    {
        if (CurrentState == newState) return;

        if (CurrentState != newState)
        {
            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);
        }
    }

    private void EnterState(PlayerStates state)
    {
        // Logic for entering the current state
        switch (state)
        {
            case PlayerStates.Idle:
                // Logic for entering Idle state
                break;
            case PlayerStates.Move:
                // Logic for entering Move state
                break;
            case PlayerStates.Jump:
                // Logic for entering Jump state
                break;
            case PlayerStates.Attack:
                // Logic for entering Attack state
                break;
            case PlayerStates.Dash:
                // Logic for entering Dash state
                break;
            case PlayerStates.Death:
                // Logic for entering Death state
                break;
        }
        Debug.Log($"Entering state: {state}");
    }

    private void ExitState(PlayerStates state)
    {
        // Logic for exiting the current state
        switch (state)
        {
            case PlayerStates.Idle:
                // Logic for exiting Idle state
                break;
            case PlayerStates.Move:
                // Logic for exiting Move state
                break;
            case PlayerStates.Jump:
                // Logic for exiting Jump state
                break;
            case PlayerStates.Attack:
                // Logic for exiting Attack state
                break;
            case PlayerStates.Dash:
                // Logic for exiting Dash state
                break;
            case PlayerStates.Death:
                // Logic for exiting Death state
                break;
        }
        Debug.Log($"Exiting state: {state}");
    }
}
