using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputReader : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnMoveCanceled;
    public event Action OnJump;

    private Vector2 moveInput;
    private bool moving;

    private void Update()
    {
        if (moving)
            OnMove?.Invoke(moveInput);
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
            moving = true;
        }
        else if (ctx.canceled)
        {
            moveInput = Vector2.zero;
            moving = false;
            OnMoveCanceled?.Invoke();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnJump?.Invoke();
    }
}
