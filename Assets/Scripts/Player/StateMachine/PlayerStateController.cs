using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerStateController : MonoBehaviour
{
    public PlayerBaseState CurrentState => _currentState;

    public PlayerMotor Motor { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputReader Input { get; private set; }
    public StatController Stat { get; private set; }
    public ActionController Actions { get; private set; }


    private PlayerBaseState _currentState;
    private Vector2 _currentMoveInput;
    private Dictionary<LocomotionState, PlayerBaseState> _states;

    private void Awake()
    {
        Motor = GetComponent<PlayerMotor>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInputReader>();
        Actions = GetComponent<ActionController>();
        Stat = GetComponent<StatController>();

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

    private void Update() => _currentState?.OnUpdate();

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

    public void SetState(LocomotionState state)
    {
        if (_states.TryGetValue(state, out PlayerBaseState newState))
        {
            if (_currentState == newState) return;

            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}