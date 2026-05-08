using UnityEngine;

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
    }

    public Vector2 GetMovementVector()
    {
        Vector2 _moveVector = _playerInputs.Player.Move.ReadValue<Vector2>();

        return _moveVector;
    }
}
