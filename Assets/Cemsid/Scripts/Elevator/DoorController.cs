using UnityEngine;
using System.Collections;

public class ElevatorDoorController : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _dingSound;
    [SerializeField] private AudioClip _rideSound;

    private Coroutine _openingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_openingCoroutine != null) StopCoroutine(_openingCoroutine);
            _openingCoroutine = StartCoroutine(OpenDoorSequence());
        }
    }

    private IEnumerator OpenDoorSequence()
    {
        if (_audioSource != null && _rideSound != null)
        {
            _audioSource.clip = _rideSound;
            _audioSource.Play();
            
            // Wait for the ride sound to finish
            yield return new WaitForSeconds(_rideSound.length);
        }

        // Open the door
        _doorAnimator.SetTrigger("Open");

        // Play the ding sound when it opens
        if (_audioSource != null && _dingSound != null)
        {
            _audioSource.PlayOneShot(_dingSound);
        }
        
        _openingCoroutine = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_openingCoroutine != null)
            {
                StopCoroutine(_openingCoroutine);
                _openingCoroutine = null;
            }
            _doorAnimator.SetTrigger("Close");
        }
    }
}
