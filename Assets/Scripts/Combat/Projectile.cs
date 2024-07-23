using RPG.Attributes;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _isHoming;
        [SerializeField] private GameObject _hitEffect;

        private const float LifeTime = 5f;
        private Health _target;
        private float _damage;
        private GameObject _instigator;

        private void Start()
        {
            AimAtTarget(_target.transform);
        }

        private void AimAtTarget(Transform target)
        {
            transform.LookAt(GetAimLocation(target.transform));
        }

        private Vector3 GetAimLocation(Transform target)
        {
            var targetCapsule = target.GetComponent<CapsuleCollider>();
            if (!targetCapsule) return target.position;

            return target.position + Vector3.up * targetCapsule.height / 2;
        }

        private void Update()
        {
            if (!_target) return;

            if (_isHoming && !_target.IsDead) AimAtTarget(_target.transform);
            transform.Translate(_speed * Time.deltaTime * Vector3.forward);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target || _target.IsDead) return;

            _target.TakeDamage(_instigator, _damage);
            if (_hitEffect) SpawnHitEffect();
            HideProjectile();
            _target = null;
        }

        private void SpawnHitEffect()
        {
            GameObject hitEffect = Instantiate(_hitEffect, GetAimLocation(_target.transform), transform.rotation);
            StartCoroutine(DestroyHitEffect(hitEffect));
        }

        private IEnumerator DestroyHitEffect(GameObject hitEffect)
        {
            var particleSystem = hitEffect.GetComponent<ParticleSystem>();
            while (particleSystem.IsAlive())
            {
                yield return null;
            }
            Destroy(hitEffect);
        }

        private void HideProjectile()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _instigator = instigator;
            _damage = damage;

            Destroy(gameObject, LifeTime);
        }
    }
}
