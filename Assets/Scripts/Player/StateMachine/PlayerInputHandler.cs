using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    //Cached input values
    public Vector2 MoveInput { get; private set; }
    //public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool DashPressed { get; private set; }

    void Awake()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();

            if (playerInput == null)
            {
                Debug.LogError("PlayerInput component not found on " + gameObject.name);
            }
        }

        //Bind callbacks directly from the InputSystem_Actions
        playerInput.actions["Move"].performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => MoveInput = Vector2.zero;

       // playerInput.actions["Look"].performed += ctx => LookInput = ctx.ReadValue<Vector2>();
       // playerInput.actions["Look"].canceled += ctx => LookInput = Vector2.zero;

        playerInput.actions["Jump"].performed += ctx => JumpPressed = true;
        playerInput.actions["Jump"].canceled += ctx => JumpPressed = false;

        playerInput.actions["Attack"].performed += ctx => AttackPressed = true;
        playerInput.actions["Attack"].canceled += ctx => AttackPressed = false;

        playerInput.actions["Dash"].performed += ctx => DashPressed = true;
        playerInput.actions["Dash"].canceled += ctx => DashPressed = false;

    }
}
