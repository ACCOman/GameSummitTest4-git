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
