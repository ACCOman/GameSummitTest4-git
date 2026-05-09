using UnityEngine;

public class FootstepSoundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStateController _playerStateController;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _footstepClip;

    [Header("Settings")]
    [SerializeField] private float _walkPitch = 1f;
    [SerializeField] private float _runPitch = 1.3f;
    [SerializeField] private float _volume = 0.5f;

    private void Awake()
    {
        if (_playerStateController == null) _playerStateController = GetComponent<PlayerStateController>();
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        
        _audioSource.clip = _footstepClip;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.volume = _volume;
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        PlayerState currentState = _playerStateController.GetCurrentPlayerState();

        if (currentState == PlayerState.walk || currentState == PlayerState.run)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

            _audioSource.pitch = (currentState == PlayerState.run) ? _runPitch : _walkPitch;
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}
