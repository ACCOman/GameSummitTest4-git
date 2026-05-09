using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {get; private set; }

    private InputHandler _inputHandler;
    private Rigidbody _playerRigidbody;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] public float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _lookSensitivity = 100f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Camera")]
    [SerializeField] private Transform _playerCamera;
    private float _xRotation = 0f;

    [Header("What Is Doing?")]
    public bool _isIdling;
    public bool _isWalking;
    public bool _isRunning;

    [Header("Ground Detection")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private bool _isGrounded;

    Vector3 _moveDirection;

    private void Awake()
    {
        Instance = this;

        _inputHandler = GetComponent<InputHandler>();
        _playerRigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;

        _isIdling = true;
        _isWalking = false;
        _isRunning = false;

        _moveSpeed = _walkSpeed;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _whatIsGround);

        PlayerMovement();
        PlayerLook();
        IsWhatDoing();
    }

    private void PlayerMovement()
    {
        Vector2 _inputVector = _inputHandler.GetMovementVector();
        _moveDirection = transform.forward * _inputVector.y + transform.right * _inputVector.x;
        _moveDirection.Normalize();

        _playerRigidbody.MovePosition(_playerRigidbody.position + _moveDirection * _moveSpeed * Time.deltaTime);

    }

    private void PlayerLook()
    {
        Vector2 _lookVector = _inputHandler.GetLookVector();

        float mouseX = _lookVector.x * _lookSensitivity* Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = _lookVector.y * _lookSensitivity * Time.deltaTime;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, 15f, 70f);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    public void HandleRun(bool isPressing)
    {
        if(isPressing)
        {
            _moveSpeed = _runSpeed;
        }

        else
        {
            _moveSpeed = _walkSpeed;
        }
    }

    public void HandleJump()
    {
        if (IsGrounded())
        {
            _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

    }

    private void IsWhatDoing()
    {
        if(_moveDirection.magnitude <= 0.1f)
        {
            _isIdling = true;
            _isWalking = false;
            _isRunning = false;
        }
        else if(_moveDirection.magnitude > 0.1f && _moveSpeed == 5f)
        {
            _isIdling = false;
            _isWalking = true;
            _isRunning = false;
        }
        else if(_moveSpeed == 10f)
        {
            _isIdling = false;
            _isWalking = false;
            _isRunning = true;
        }
    }

    private bool IsGrounded()
    {
        return _isGrounded;
    }
}
