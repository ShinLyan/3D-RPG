using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
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
        private LazyValue<Weapon> _currentWeapon;

        private Weapon CurrentWeapon
        {
            get => _currentWeapon.Value;
            set => _currentWeapon.Value = value;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
            _currentWeapon = new(SetupDefaultWeapon);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

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

        #region Weapons
        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }

        private void AttachWeapon(Weapon weapon)
        {
            var animator = GetComponent<Animator>();
            weapon.Spawn(_leftHandTransform, _rightHandTransform, animator);
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            AttachWeapon(weapon);
        }

        private Weapon LoadWeapon(string weaponName) => Resources.Load<Weapon>(weaponName);
        #endregion

        private bool IsInAttackRange() =>
            Vector3.Distance(transform.position, Target.transform.position) < CurrentWeapon.AttackRange;

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

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (CurrentWeapon.HasProjectTile())
            {
                CurrentWeapon.LaunchProjecttile(
                    _leftHandTransform, _rightHandTransform, Target, gameObject, damage);
            }
            else
            {
                Target.TakeDamage(gameObject, damage);
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

        #region IModifierProvider
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.PercentageDamageBonus;
            }
        }
        #endregion

        #region ISaveable
        public object CaptureState() => CurrentWeapon.name;

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            EquipWeapon(LoadWeapon(weaponName));
        }
        #endregion
    }
}
