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
    private Vector2 currentMoveInput;
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
            { LocomotionState.Dash, new DashState(this) }
        };
    }

    private void OnEnable()
    {
        Input.OnMove += HandleMove;
        Input.OnMoveCanceled += HandleMoveCanceled;
        Input.OnJump += HandleJump;
        Input.OnDash += HandleDash;

        Motor.OnLanded += OnLanded;
        Motor.OnAirborne += OnAirborne;
        Motor.OnDashEnded += OnDashEnded; // Subscribe to the new event
    }

    private void OnDisable()
    {
        Input.OnMove -= HandleMove;
        Input.OnMoveCanceled -= HandleMoveCanceled;
        Input.OnJump -= HandleJump;
        Input.OnDash -= HandleDash;

        Motor.OnLanded -= OnLanded;
        Motor.OnAirborne -= OnAirborne;
        Motor.OnDashEnded -= OnDashEnded; // Unsubscribe
    }

    private void Start()
    {
        SetState(LocomotionState.Idle);
    }

    private void Update() => currentState?.OnUpdate();

    private void HandleMove(Vector2 moveInput)
    {
        currentMoveInput = moveInput;
        currentState?.OnInput(moveInput);
    }

    private void HandleMoveCanceled()
    {
        currentMoveInput = Vector2.zero;
        currentState?.OnInput(Vector2.zero);
    }

    private void HandleJump() => currentState?.HandleJumpAttempt();
    private void HandleDash() => currentState?.HandleDashAttempt();

    private void OnLanded() => currentState?.OnLanded();
    private void OnAirborne() => SetState(LocomotionState.Fall);

    // Let the current state know the dash has finished.
    private void OnDashEnded() => currentState?.OnDashEnded();

    public Vector2 GetCurrentMoveInput() => currentMoveInput;

    public void SetState(LocomotionState state)
    {
        if (states.TryGetValue(state, out PlayerBaseState newState))
        {
            if (currentState == newState) return;

            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}