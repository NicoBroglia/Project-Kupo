// PlayerStateController.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerStateController : MonoBehaviour
{
    public PlayerMotor Motor { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputReader Input { get; private set; }

    private PlayerBaseState currentState;
    private Dictionary<LocomotionState, PlayerBaseState> states;

    private void Awake()
    {
        Motor = GetComponent<PlayerMotor>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInputReader>();

        states = new Dictionary<LocomotionState, PlayerBaseState>
        {
            { LocomotionState.Idle, new IdleState(this) },
            { LocomotionState.Move, new MoveState(this) },
            { LocomotionState.Jump, new JumpState(this) },
            { LocomotionState.Fall, new FallState(this) },
        };
    }

    private void OnEnable()
    {
        Input.OnMove += HandleMove;
        Input.OnMoveCanceled += HandleMoveCanceled;
        Input.OnJump += HandleJump;

        Motor.OnLanded += OnLanded;
        Motor.OnAirborne += OnAirborne;
    }

    private void OnDisable()
    {
        Input.OnMove -= HandleMove;
        Input.OnMoveCanceled -= HandleMoveCanceled;
        Input.OnJump -= HandleJump;

        Motor.OnLanded -= OnLanded;
        Motor.OnAirborne -= OnAirborne;
    }

    private void Update() => currentState?.OnUpdate();

    private void HandleMove(Vector2 moveInput) =>
        currentState?.OnInput(moveInput);

    private void HandleMoveCanceled() =>
        SetState(LocomotionState.Idle);

    private void HandleJump() =>
        SetState(LocomotionState.Jump);

    private void OnLanded() =>
        SetState(LocomotionState.Idle);

    private void OnAirborne() =>
        SetState(LocomotionState.Fall);

    public void SetState(LocomotionState state)
    {
        if (currentState == states[state]) return;
        currentState?.OnExit();
        currentState = states[state];
        currentState.OnEnter();
    }

    private void Start() =>
        SetState(LocomotionState.Idle);
}
