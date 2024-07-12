using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private float _weaponDamage = 10f;

        private Transform _target;
        private float _timeSinceLastAttack = 0;

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (!_target) return;

            if (GetIsInRange())
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
            else
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                // Запускает Hit() Animation Event
                const string TriggerName = "Attack";
                GetComponent<Animator>().SetTrigger(TriggerName);
                _timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        private void Hit()
        {
            if (!_target) return;
            var healthComponent = _target.GetComponent<Health>();
            healthComponent.TakeDamage(_weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}
