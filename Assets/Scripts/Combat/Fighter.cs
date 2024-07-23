using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Animator _animator;
        private Mover _mover;
        public Health Target { get; private set; }

        [Header("Weapons")]
        [SerializeField] private Weapon _defaultWeapon;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Transform _rightHandTransform;
        private Weapon _currentWeapon;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();

            if (!_currentWeapon) EquipWeapon(_defaultWeapon);
        }

        #region Weapons
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            var animator = GetComponent<Animator>();
            weapon.Spawn(_leftHandTransform, _rightHandTransform, animator);
        }

        private Weapon LoadWeapon(string weaponName) => Resources.Load<Weapon>(weaponName);
        #endregion

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (!Target || Target.IsDead) return;

            if (IsInAttackRange())
            {
                _mover.Cancel();
                AttackBehaviour();
            }
            else
            {
                _mover.MoveTo(Target.transform.position);
            }
        }

        private bool IsInAttackRange() =>
            Vector3.Distance(transform.position, Target.transform.position) < _currentWeapon.AttackRange;

        private void AttackBehaviour()
        {
            transform.LookAt(Target.transform);

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

        #region Animation Events
        private void Hit()
        {
            if (!Target) return;

            if (_currentWeapon.HasProjectTile())
            {
                _currentWeapon.LaunchProjecttile(
                    _leftHandTransform, _rightHandTransform, Target, gameObject);
            }
            else
            {
                Target.TakeDamage(gameObject, _currentWeapon.Damage);
            }
        }

        private void Shoot() => Hit();
        #endregion

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget) return false;

            var target = combatTarget.GetComponent<Health>();
            return combatTarget && target && !target.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            Target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            const string TriggerName1 = "Attack";
            _animator.ResetTrigger(TriggerName1);

            const string TriggerName2 = "StopAttack";
            _animator.SetTrigger(TriggerName2);
        }

        #region Saving
        public object CaptureState() => _currentWeapon.name;

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            EquipWeapon(LoadWeapon(weaponName));
        }
        #endregion
    }
}
