public class IdleState : IState
{
    private PlayerController controller;
    private StateController stateController;

    public IdleState(PlayerController controller, StateController stateController)
    {
        this.controller = controller;
        this.stateController = stateController;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update()
    {
        if (controller.Input.MoveInput.sqrMagnitude > 0.01f)
            stateController.SetState(LocomotionState.Move);
    }

    public bool CanProcess(ICommand command) => true;
}
