using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public int Vertical { get; private set; }
    public int Horizontal { get; private set; }
    public bool JumpJustPressed { get; private set; }
    public bool JumpBeingHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool AttackInput { get; private set; }
    public bool DashInput { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _verticalAction;
    private InputAction _horizontalAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _dashAction;

    private void Awake(){
        if(instance == null) instance = this;

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }
    private void Update(){
        UpdateInputs();
    }
    private void SetupInputActions(){
        _verticalAction = _playerInput.actions["Vertical"];
        _horizontalAction = _playerInput.actions["Horizontal"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
        _dashAction = _playerInput.actions["Dash"];
    }

    private void UpdateInputs(){
        Vertical = _verticalAction.ReadValue<int>();
        Horizontal = _horizontalAction.ReadValue<int>();
        JumpJustPressed = _jumpAction.WasPressedThisFrame();
        JumpBeingHeld = _jumpAction.IsPressed();
        JumpReleased = _jumpAction.WasReleasedThisFrame();
        AttackInput = _attackAction.WasPressedThisFrame();
        DashInput = _dashAction.WasPressedThisFrame();
    }
}   
