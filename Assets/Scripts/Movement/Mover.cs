using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform _target;

        private NavMeshAgent _navMeshAgent;
        private Health _health;


        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            const string ForwardSpeed = "ForwardSpeed";
            GetComponent<Animator>().SetFloat(ForwardSpeed, speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
