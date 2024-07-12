using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;

        private bool _isDead = false;

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            const string TriggerName = "Die";
            GetComponent<Animator>().SetTrigger(TriggerName);
        }
    }
}
