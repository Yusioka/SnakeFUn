using UnityEngine;
using UnityEngine.AI;

namespace Snakefun.Game.Control
{
    public class MenuSnakeController : MonoBehaviour
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float speed = 0.2f;

        NavMeshAgent navMeshAgent;
        Quaternion startRotation;
        Vector3 guardPosition;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            startRotation = transform.rotation;
            guardPosition = transform.position;
        }

        private void Start()
        {
            navMeshAgent.stoppingDistance = waypointTolerance;
        }

        private void Update()
        {
            PatrolBehaviour();
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        public void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint() && waypointDwellTime != 0)
                {
                    navMeshAgent.velocity = Vector3.zero;
                    navMeshAgent.isStopped = true;
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                else if (AtWaypoint() && waypointDwellTime == 0)
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                else
                {
                    navMeshAgent.isStopped = false;
                    nextPosition = GetCurrentWaypoint();
                }
            }
            else
            {
                transform.rotation = startRotation;
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                if (gameObject.GetComponent<NavMeshAgent>().enabled)
                {
                    MoveTo(nextPosition, speed);
                }
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }


        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = this.speed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }
    }
}
