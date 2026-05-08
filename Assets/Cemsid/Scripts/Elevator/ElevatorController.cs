using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform[] _floors;

    [Header("Settings")]
    [SerializeField] private float _speed = 2f;

    private Transform targetFloor;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        TargetFloor();
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

    public void GoToFloor(int floorIndex)
    {
        if (floorIndex >= 0 && floorIndex < _floors.Length)
        {
            targetFloor = _floors[floorIndex];
        }
    }
}
