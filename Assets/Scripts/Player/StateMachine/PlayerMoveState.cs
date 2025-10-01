using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateController stateController;
    private readonly PlayerInputHandler input;

    public PlayerMoveState(PlayerController controller, PlayerStateController stateController, PlayerInputHandler input)
    {
        this.controller = controller;
        this.stateController = stateController;
        this.input = input;
    }

    public void Enter() { Debug.Log("Enter Move"); }

    public void Update()
    {
        // Move the player using camera-relative movement
        controller.MovePlayer(input.MoveInput);

        Debug.Log("MoveState Update");

        // Return to Idle if input stops
        if (input.MoveInput.sqrMagnitude < 0.01f)
        {
            stateController.SetState(PlayerStates.Idle);
        }

        // Jump or attack transitions
        if (input.JumpPressed)
            stateController.SetState(PlayerStates.Jump);

        if (input.AttackPressed)
            stateController.SetState(PlayerStates.Attack);
    }

    public void Exit() { Debug.Log("Exit Move"); }
}
