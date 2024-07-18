using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject _equippedPrefab;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;

        [SerializeField] private float _damage;
        [SerializeField] private float _attackRange;
        [SerializeField] private bool _isRightHanded = true;

        public float Damage => _damage;
        public float AttackRange => _attackRange;

        public void Spawn(Transform leftHand, Transform rightHand, Animator animator)
        {
            if (_equippedPrefab)
            {
                Transform handTransform = _isRightHanded ? rightHand : leftHand;
                Instantiate(_equippedPrefab, handTransform);
            }
            if (_animatorOverrideController)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
        }



    }
}

