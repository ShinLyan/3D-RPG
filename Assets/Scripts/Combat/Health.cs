using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;

        // private bool _isDead = false;

        public bool IsDead { get; private set; }

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
            if (IsDead) return;

            IsDead = true;
            const string TriggerName = "Die";
            GetComponent<Animator>().SetTrigger(TriggerName);
        }
    }
}
