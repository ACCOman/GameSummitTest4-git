using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler _inputHandler;
    private Rigidbody _playerRigidbody;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 10f;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 _inputVector = _inputHandler.GetMovementVector();
        Vector3 _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y);

        _playerRigidbody.MovePosition(_playerRigidbody.position + _moveDirection * _moveSpeed * Time.deltaTime);

    }
}
