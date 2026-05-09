using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform[] _floors;

    [Header("Settings")]
    [SerializeField] private float _speed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _workingSound;

    private Transform targetFloor;

    private void Awake()
    {
        Instance = this;
        
        if (_audioSource != null && _workingSound != null)
        {
            _audioSource.clip = _workingSound;
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        TargetFloor();
        HandleAudio();
    }

    private void TargetFloor()
    {
        if (targetFloor != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetFloor.position,
                _speed * Time.deltaTime
            );
        }
    }

    private void HandleAudio()
    {
        if (_audioSource == null || _workingSound == null || targetFloor == null) return;

        bool isMoving = Vector3.Distance(transform.position, targetFloor.position) > 0.01f;

        if (isMoving)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

    public void GoToFloor(int floorIndex)
    {
        if (floorIndex >= 0 && floorIndex < _floors.Length)
        {
            targetFloor = _floors[floorIndex];
        }
    }
}
