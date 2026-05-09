using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Inputs _playerInputs;

    private void Awake()
    {
        _playerInputs = new Inputs();
    }

    private void OnEnable()
    {
        _playerInputs.Player.Enable();

        _playerInputs.Player.Run.started += ctx => PlayerController.Instance.HandleRun(true);
        _playerInputs.Player.Run.canceled += ctx => PlayerController.Instance.HandleRun(false);
        _playerInputs.Player.Jump.performed += PlayerJump;
        _playerInputs.Player.Pick.performed += ctx => PickItems.Instance.TryPickup();
        _playerInputs.Player.Drop.performed += ctx => PickItems.Instance.DropItem();

        _playerInputs.Elevator.Enable();
        
        _playerInputs.Elevator._1.performed += ctx => ElevatorController.Instance.GoToFloor(0);
        _playerInputs.Elevator._2.performed += ctx => ElevatorController.Instance.GoToFloor(1);
        _playerInputs.Elevator._3.performed += ctx => ElevatorController.Instance.GoToFloor(2);
        _playerInputs.Elevator._4.performed += ctx => ElevatorController.Instance.GoToFloor(3);
        _playerInputs.Elevator._5.performed += ctx => ElevatorController.Instance.GoToFloor(4);
        _playerInputs.Elevator._6.performed += ctx => ElevatorController.Instance.GoToFloor(5);
        _playerInputs.Elevator._7.performed += ctx => ElevatorController.Instance.GoToFloor(6);
    }

    private void PlayerJump(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            PlayerController.Instance.HandleJump();
        }
    }

    public Vector2 GetMovementVector()
    {
        Vector2 _moveVector = _playerInputs.Player.Move.ReadValue<Vector2>();

        return _moveVector;
    }

    public Vector2 GetLookVector()
    {
        Vector2 _lookVector = _playerInputs.Player.Look.ReadValue<Vector2>();

        return _lookVector;
    }
}
