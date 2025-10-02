using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private StateController stateController;

    // Cached input values
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool DashPressed { get; private set; }

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction dashAction;
    private InputAction jumpAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        stateController = GetComponent<StateController>();

        if (playerInput == null)
            Debug.LogError("PlayerInput component not found on " + gameObject.name);

        // Cache action references
        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
        dashAction = playerInput.actions["Dash"];
       // if (playerInput.actions.Contains("Jump"))
      //     jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        // Movement is continuous
        MoveInput = moveAction.ReadValue<Vector2>();

        // One-frame presses (edge detection)
        AttackPressed = attackAction.WasPerformedThisFrame();
        DashPressed = dashAction.WasPerformedThisFrame();
        JumpPressed = jumpAction != null && jumpAction.WasPerformedThisFrame();

        // Queue commands if pressed
        if (AttackPressed) stateController.EnqueueCommand(new AttackCommand());
        if (DashPressed) stateController.EnqueueCommand(new DashCommand());
        // If you add JumpCommand later:
        // if (JumpPressed) stateController.EnqueueCommand(new JumpCommand());
    }
}
