using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _player;

    [Header("Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _detectionRadius = 10f;

    private NavMeshAgent _agent;
    private bool isChasing = false;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Chasing();
    }

    private void Chasing()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        if (!isChasing && distance <= _detectionRadius)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            _agent.SetDestination(_player.position);
            _agent.speed = _speed;
            _animator.SetBool("_isChasing", true);
        }
    }
}
