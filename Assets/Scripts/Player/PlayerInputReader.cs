using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputReader : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnMoveCanceled;
    public event Action OnJump;
    public event Action OnDash;

    // Fields 'moveInput' and 'moving' removed.
    // The Update method is removed.

    // Input events are now fired immediately upon change.
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Firing the event directly with the value.
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }
        else if (ctx.canceled)
        {
            // Input is zeroed, fire the cancel event.
            OnMoveCanceled?.Invoke();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnJump?.Invoke();
    }

    public void OnDashInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnDash?.Invoke();
    }
}