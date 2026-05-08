using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _playerAnimator;

    private PlayerStateController _playerStateController;

    private void Awake()
    {
        _playerStateController = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        SetPlayerAnimation();
    }

    private void SetPlayerAnimation()
    {
        var _currentState = _playerStateController.GetCurrentPlayerState();

        switch(_currentState)
        {
            case PlayerState.idle:
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_IDLING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_WALKING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_RUNNING, false);
                break;
            
            case PlayerState.walk:
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_IDLING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_WALKING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_RUNNING, false);
                break;

            case PlayerState.run:
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_IDLING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_WALKING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimation.IS_RUNNING, true);
                break;
        }
    }
}
