using UnityEngine;

public class ElevatorDoorController : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _doorAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _doorAnimator.SetTrigger("Close");
        }
    }
}
