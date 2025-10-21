using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputReader : MonoBehaviour
{
    // C# events provide a robust way for other scripts to subscribe to input actions.
    public event Action<Vector2> OnMove = delegate { };
    public event Action OnMoveCanceled = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnDash = delegate { };

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnMove.Invoke(ctx.ReadValue<Vector2>());
        }
        else if (ctx.canceled)
        {
            OnMoveCanceled.Invoke();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnJump.Invoke();
    }

    public void OnDashInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnDash.Invoke();
    }
}
