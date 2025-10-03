public class MoveState : IState
{
    private PlayerController controller;
    private StateController stateController;

    public MoveState(PlayerController controller, StateController stateController)
    {
        this.controller = controller;
        this.stateController = stateController;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update()
    {
        controller.Motor.Move(controller.Input.MoveInput);

        if (controller.Input.MoveInput.sqrMagnitude < 0.01f)
            stateController.SetState(LocomotionState.Idle);
    }

    public bool CanProcess(ICommand command) => true;

}
