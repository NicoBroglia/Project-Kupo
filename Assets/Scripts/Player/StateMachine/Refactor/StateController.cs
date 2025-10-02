using UnityEngine;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    private Dictionary<LocomotionState, IState> states;
    private IState currentState;

    public LocomotionState CurrentStateType { get; private set; }

    private Queue<ICommand> commandQueue = new Queue<ICommand>();
    private PlayerController controller;

    public void Initialize(PlayerController controller, Dictionary<LocomotionState, IState> stateMap)
    {
        this.controller = controller;
        states = stateMap;
        SetState(LocomotionState.Idle);
    }

    public void SetState(LocomotionState newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        CurrentStateType = newState;
        currentState.Enter();
    }

    public void EnqueueCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    private void Update()
    {
        currentState?.Update();

        while (commandQueue.Count > 0)
        {
            var cmd = commandQueue.Dequeue();
            if (currentState.CanProcess(cmd))
                cmd.Execute(controller);
        }
    }
}
