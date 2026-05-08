using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerState _currentState;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _currentState = PlayerState.idle;
    }

    private void Update()
    {
        SetPlayerState();
    }

    private void SetPlayerState()
    {
        switch(_currentState)
        {
            case PlayerState.idle:
                if(_playerController._isWalking && !_playerController._isIdling && !_playerController._isRunning)
                {
                    _currentState = PlayerState.walk;
                }
                if(_playerController._isRunning && !_playerController._isIdling && !_playerController._isWalking)
                {
                    _currentState = PlayerState.run;
                }
                break;

            case PlayerState.walk:
                if(_playerController._isIdling && !_playerController._isWalking && !_playerController._isRunning)
                {
                    _currentState = PlayerState.idle;
                }
                if(_playerController._isRunning && !_playerController._isIdling && !_playerController._isWalking)
                {
                    _currentState = PlayerState.run;
                }
                break;

            case PlayerState.run:

                if(_playerController._isIdling && !_playerController._isWalking && !_playerController._isRunning)
                {
                    _currentState = PlayerState.idle;
                }
                if(_playerController._isWalking && !_playerController._isIdling && !_playerController._isRunning)
                {
                    _currentState = PlayerState.walk;
                }
                break;
        }
    }

    public PlayerState GetCurrentPlayerState()
    {
        return _currentState;
    }
}
