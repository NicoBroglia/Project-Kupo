using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateController stateController;
    private readonly PlayerInputHandler input;

    public PlayerIdleState(PlayerController controller, PlayerStateController stateController, PlayerInputHandler input)
    {
        this.controller = controller;
        this.stateController = stateController;
        this.input = input;
    }

    public void Enter() { Debug.Log("Enter Idle"); }

    public void Update()
    {
        // Debug every frame to confirm Update is running
        Debug.Log("IdleState Update");

        // Check if player is giving movement input
        if (input.MoveInput.sqrMagnitude > 0.01f)
        {
            Debug.Log("Switching to MoveState");
            stateController.SetState(PlayerStates.Move);
        }

        // You can also handle jump or attack transitions here
        if (input.JumpPressed)
            stateController.SetState(PlayerStates.Jump);

        if (input.AttackPressed)
            stateController.SetState(PlayerStates.Attack);
    }

    public void Exit() { Debug.Log("Exit Idle"); }
}
