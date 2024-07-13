using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;

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
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
