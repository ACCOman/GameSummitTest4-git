using UnityEngine;
using UnityEngine.AI;

namespace S.Omer.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCMovement : MonoBehaviour
    {
        [Header("Chasing Settings")]
        [SerializeField] private string targetPath = "Player/HumanM_Model/HumanM_BodyMesh";
        [SerializeField] private float detectionRange = 20f;
        [SerializeField] private float stopDistance = 1.5f;
        [SerializeField] private float updateRate = 0.2f;

        [Header("Patrol Settings (Fallback)")]
        [SerializeField] private float patrolRadius = 10f;
        [SerializeField] private float waitTime = 2f;

        private NavMeshAgent agent;
        private Transform target;
        private float nextUpdateTime;
        private float waitTimer;
        private bool isChasing;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = stopDistance;
            
            GameObject targetObj = GameObject.Find(targetPath);
            if (targetObj != null)
            {
                target = targetObj.transform;
            }
            else
            {
                Debug.LogWarning($"NPCMovement: Target '{targetPath}' not found.");
            }
        }

        void Update()
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= detectionRange)
            {
                isChasing = true;
                HandleChasing();
            }
            else
            {
                isChasing = false;
                HandlePatrol();
            }
        }

        private void HandleChasing()
        {
            if (Time.time >= nextUpdateTime)
            {
                agent.SetDestination(target.position);
                nextUpdateTime = Time.time + updateRate;
            }
        }

        private void HandlePatrol()
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    MoveToRandomPoint();
                    waitTimer = 0;
                }
            }
        }

        public void MoveToRandomPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Detection Range
            Gizmos.color = isChasing ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            // Patrol Radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
        #endif
    }
}

