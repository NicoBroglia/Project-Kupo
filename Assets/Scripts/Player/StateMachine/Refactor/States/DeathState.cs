using UnityEngine;

public class DeathState : IState
{
    private PlayerController controller;
    private StateController stateController;

    public DeathState(PlayerController controller, StateController stateController)
    {
        this.controller = controller;
        this.stateController = stateController;
    }
    public void Enter() 
    {
        var anim = controller.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Death");

        controller.Motor?.Stop();
    }
    public void Exit() { }

    public void Update()
    {
        // stateController.SetState(LocomotionState.Death);
    }

    public bool CanProcess(ICommand command) => false; //cannot process any commands in death state
}
