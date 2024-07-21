using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPoints = 100f;

        private float HealthPoints
        {
            set
            {
                _healthPoints = value;
                if (value == 0) Die();
            }
        }

        public bool IsDead { get; private set; }

        public void TakeDamage(float damage)
        {
            HealthPoints = Mathf.Max(_healthPoints - damage, 0);
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            const string TriggerName = "Die";
            GetComponent<Animator>().SetTrigger(TriggerName);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        #region Saving
        public object CaptureState() => _healthPoints;

        public void RestoreState(object state) => HealthPoints = (float)state;
        #endregion
    }
}
