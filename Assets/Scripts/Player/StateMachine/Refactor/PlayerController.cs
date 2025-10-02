using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputHandler Input { get; private set; }
    public PlayerMotor Motor { get; private set; }
    public AbilitySystem AbilitySystem { get; private set; }
    private StateController stateController;

    void Awake()
    {
        Input = GetComponent<PlayerInputHandler>();
        Motor = GetComponent<PlayerMotor>();
        AbilitySystem = new AbilitySystem();
        stateController = GetComponent<StateController>();

        // Register abilities
        var animator = GetComponent<Animator>();
        AbilitySystem.RegisterAbility("Attack", new AttackAbility(animator));
        AbilitySystem.RegisterAbility("Dash", new DashAbility(animator, Motor));

        // Setup states
        var stateMap = new Dictionary<LocomotionState, IState>
        {
            { LocomotionState.Idle, new IdleState(this, stateController) },
            { LocomotionState.Move, new MoveState(this, stateController) },
            { LocomotionState.Death, new DeathState(this, stateController) }
        };
        stateController.Initialize(this, stateMap);
    }

    // Removed RotateTowards: rotation is now fully handled by PlayerMotor
}
