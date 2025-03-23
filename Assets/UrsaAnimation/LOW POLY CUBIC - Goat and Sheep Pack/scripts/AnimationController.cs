using UnityEngine;
using UnityEngine.AI;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimalAIController : MonoBehaviour
    {
        public Animator animator;
        public NavMeshAgent agent;
        public string walkForwardAnimation = "walk_forward";
        public string idleAnimation = "idle";
        public string runForwardAnimation = "run_forward";
        public float detectionRadius = 5f;
        public float runDuration = 3f;
        public float stopDistance = 15f; // Kho?ng cách t?i ða trý?c khi d?ng
        public Transform player;
        public LayerMask obstacleMask;

        private bool isRunningAway = false;
        private float runTimer = 0f;

        void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        void Update()
        {
            if (isRunningAway)
            {
                RunAway();
            }
            else
            {
                DetectPlayer();
            }
        }

        void DetectPlayer()
        {
            if (player == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < detectionRadius)
            {
                StartRunningAway();
            }
        }

        void StartRunningAway()
        {
            isRunningAway = true;
            runTimer = runDuration;
            Vector3 runDirection = (transform.position - player.position).normalized;
            Vector3 runTarget = transform.position + runDirection * 10f;

            if (Physics.Raycast(transform.position, runDirection, 5f, obstacleMask))
            {
                runDirection = Quaternion.Euler(0, 90, 0) * runDirection;
                runTarget = transform.position + runDirection * 10f;
            }

            if (NavMesh.SamplePosition(runTarget, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                animator.Play(runForwardAnimation);
            }
        }

        void RunAway()
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance || distanceFromPlayer >= stopDistance)
            {
                isRunningAway = false;
                agent.ResetPath(); // D?ng di chuy?n
                animator.Play(idleAnimation); // Chuy?n v? tr?ng thái idle
            }
        }
    }
}
