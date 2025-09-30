using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerStates CurrentState { get; private set; }

    PlayerController movement;
    PlayerInputHandler input;
    PlayerAnimationHandler anim;

    void Awake()
    {
        movement = GetComponent<PlayerController>();
        input = GetComponent<PlayerInputHandler>();
        anim = GetComponent<PlayerAnimationHandler>();
    }

    void Start()
    {
        SetState(PlayerStates.Idle);
    }

    void Update()
    {
        UpdateState(CurrentState);
        movement.ApplyGravity(); // Always apply gravity
    }

    public void SetState(PlayerStates newState)
    {
        if (CurrentState == newState) return;

        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(CurrentState);
        Debug.Log($"Switched to: {newState}");
    }

    private void EnterState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.Idle:
                anim?.SetIdle(true);
                break;
            case PlayerStates.Move:
                anim?.SetMoving(true);
                break;
            case PlayerStates.Jump:
                movement.Jump();
                anim?.SetJump();
                break;
        }
    }

    private void ExitState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.Idle:
                anim?.SetIdle(false);
                break;
            case PlayerStates.Move:
                anim?.SetMoving(false);
                break;
        }
    }

    private void UpdateState(PlayerStates state)
    {
        switch (state)
        {
            case PlayerStates.Idle:
                if (input.GetMoveInput().magnitude > 0.1f)
                    SetState(PlayerStates.Move);
                if (input.JumpPressed())
                    SetState(PlayerStates.Jump);
                break;

            case PlayerStates.Move:
                var moveDir = input.GetMoveInput();
                if (moveDir.magnitude > 0.1f)
                    movement.Move(moveDir);
                else
                    SetState(PlayerStates.Idle);

                if (input.JumpPressed())
                    SetState(PlayerStates.Jump);
                break;

            case PlayerStates.Jump:
                var moveDirWhileJump = input.GetMoveInput();
                if (moveDirWhileJump.magnitude > 0.1f)
                    movement.Move(moveDirWhileJump);

                if (movement.IsGrounded())
                    SetState(input.GetMoveInput().magnitude > 0.1f ? PlayerStates.Move : PlayerStates.Idle);
                break;
        }
    }
}
