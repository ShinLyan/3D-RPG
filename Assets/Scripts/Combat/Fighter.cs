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

        private Health _target;
        private float _timeSinceLastAttack = 0;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (!_target || _target.IsDead) return;

            if (GetIsInRange())
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
            else
            {
                GetComponent<Mover>().MoveTo(_target.transform.position);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                // Запускает Hit() Animation Event
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            const string TriggerName1 = "StopAttack";
            _animator.ResetTrigger(TriggerName1);

            const string TriggerName2 = "Attack";
            _animator.SetTrigger(TriggerName2);
        }

        // Animation Event
        private void Hit()
        {
            if (!_target) return;
            _target.TakeDamage(_weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _weaponRange;
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (!combatTarget) return false;

            var target = combatTarget.GetComponent<Health>();
            return combatTarget && target && !target.IsDead;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
        }

        private void StopAttack()
        {
            const string TriggerName1 = "Attack";
            _animator.ResetTrigger(TriggerName1);

            const string TriggerName2 = "StopAttack";
            _animator.SetTrigger(TriggerName2);
        }
    }
}
