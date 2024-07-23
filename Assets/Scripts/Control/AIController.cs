using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Attack Behaviour")]
        [SerializeField, Range(1f, 10f)] private float _chaseDistance = 5f;
        [SerializeField] private float _allowedDistanceDeparture = 30f;
        private Fighter _fighter;
        private GameObject _player;

        [Header("Suspition Behaviour")]
        [SerializeField] private float _suspicionTime = 5f;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        [Header("Patrol Behaviour")]
        [SerializeField] private PatrolPath _patrolPath;
        [SerializeField] private float _waypointTolerance = 1f;
        [SerializeField] private float _waypointDwellTime = 3f;
        [SerializeField, Range(0, 1)] private float _patrolSpeedFraction = 0.2f;
        private Vector3 _guardPosition;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int _currentWaypointIndex = 0;
        private Mover _mover;

        private Health _health;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _player = GameObject.FindWithTag("Player");

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player) &&
                !IsFarFromStartPosition(_guardPosition))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspitionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        #region AttackBehaviour
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < _chaseDistance;
        }

        private bool IsFarFromStartPosition(Vector3 startPosition)
        {
            return Vector3.Distance(startPosition, transform.position) > _allowedDistanceDeparture;
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }
        #endregion

        #region SuspitionBehaviour
        private void SuspitionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        #endregion

        #region PatrolBehaviour
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;

            if (_patrolPath != null && _patrolPath.Waypoints.Length > 0)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }
        #endregion

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        #region Debug
        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
        #endregion
    }
}
