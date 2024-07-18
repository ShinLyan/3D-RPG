using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;

        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Animator _animator;
        private Mover _mover;

        [Header("Weapons")]
        [SerializeField] private Weapon _defaultWeapon;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Transform _rightHandTransform;
        private Weapon _currentWeapon;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
            EquipWeapon(_defaultWeapon);
        }

        #region Weapons
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            var animator = GetComponent<Animator>();
            weapon.Spawn(_leftHandTransform, _rightHandTransform, animator);
        }
        #endregion

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (!_target || _target.IsDead) return;

            if (IsInAttackRange())
            {
                _mover.Cancel();
                AttackBehaviour();
            }
            else
            {
                _mover.MoveTo(_target.transform.position);
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
            _target.TakeDamage(_currentWeapon.Damage);
        }

        private bool IsInAttackRange() =>
            Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.AttackRange;

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget) return false;

            var target = combatTarget.GetComponent<Health>();
            return combatTarget && target && !target.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
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
