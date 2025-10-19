using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(ActionController))]
[RequireComponent(typeof(StatController))]
public class PlayerStateController : MonoBehaviour
{
    public PlayerBaseState CurrentState => _currentState;

    // Properties to expose components to the states.
    public PlayerMotor Motor { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputReader Input { get; private set; }
    public ActionController Actions { get; private set; }

    private PlayerBaseState _currentState;
    private Vector2 _currentMoveInput;
    private Dictionary<LocomotionState, PlayerBaseState> _states;

    private void Awake()
    {
        // Cache all component references.
        Motor = GetComponent<PlayerMotor>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInputReader>();
        Actions = GetComponent<ActionController>();

        // Initialize the state dictionary.
        _states = new Dictionary<LocomotionState, PlayerBaseState>
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
        // Subscribe to input and motor events.
        Input.OnMove += HandleMove;
        Input.OnMoveCanceled += HandleMoveCanceled;
        Input.OnJump += HandleJump;
        Input.OnDash += HandleDash;

        Motor.OnLanded += OnLanded;
        Motor.OnAirborne += OnAirborne;
        Motor.OnDashEnded += OnDashEnded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks.
        Input.OnMove -= HandleMove;
        Input.OnMoveCanceled -= HandleMoveCanceled;
        Input.OnJump -= HandleJump;
        Input.OnDash -= HandleDash;

        Motor.OnLanded -= OnLanded;
        Motor.OnAirborne -= OnAirborne;
        Motor.OnDashEnded -= OnDashEnded;
    }

    private void Start()
    {
        SetState(LocomotionState.Idle);
    }

    private void Update()
    {
        // Delegate the update logic to the current state.
        _currentState?.OnUpdate();
    }

    // --- Event Handlers ---
    private void HandleMove(Vector2 moveInput)
    {
        _currentMoveInput = moveInput;
        _currentState?.OnInput(moveInput);
    }

    private void HandleMoveCanceled()
    {
        _currentMoveInput = Vector2.zero;
        _currentState?.OnInput(Vector2.zero);
    }

    private void HandleJump() => _currentState?.HandleJumpAttempt();
    private void HandleDash() => Actions.TryPerformDash();
    private void OnLanded() => _currentState?.OnLanded();
    private void OnAirborne() => SetState(LocomotionState.Fall);
    private void OnDashEnded() => _currentState?.OnDashEnded();

    public Vector2 GetCurrentMoveInput() => _currentMoveInput;

    public void SetState(LocomotionState stateKey)
    {
        if (_states.TryGetValue(stateKey, out PlayerBaseState newState))
        {
            if (_currentState == newState) return;

            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}