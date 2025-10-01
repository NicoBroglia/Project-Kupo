using UnityEngine;
using System.Collections.Generic;

public class PlayerStateController : MonoBehaviour
{
    private Dictionary<PlayerStates, IPlayerState> states;
    private IPlayerState currentState;

    public PlayerStates CurrentStateType { get; private set; }

    public void InitializeStates(Dictionary<PlayerStates, IPlayerState> stateDictionary)
    {
        states = stateDictionary;

        // Force initialization
        CurrentStateType = PlayerStates.Idle;
        currentState = states[PlayerStates.Idle];
        currentState.Enter();

        Debug.Log("FSM initialized, current state: " + CurrentStateType);
    }

    public void SetState(PlayerStates newStateType)
    {
        if (!states.ContainsKey(newStateType))
        {
            Debug.LogError("State not found: " + newStateType);
            return;
        }

        if (currentState != null)
            currentState.Exit();

        currentState = states[newStateType];
        CurrentStateType = newStateType;
        currentState.Enter();

        Debug.Log("SetState called: " + newStateType);
    }

    private void Update()
    {
        currentState?.Update();
    }
}
