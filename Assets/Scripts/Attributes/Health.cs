using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float _healthPoints = -1f;
        private BaseStats _baseStats;

        public bool IsDead { get; private set; }

        private void Start()
        {
            _baseStats = GetComponent<BaseStats>();
            if (_healthPoints < 0)
            {
                _healthPoints = _baseStats.GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
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

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (!experience) return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        #region Display
        public float GetPercentage() => 100 * _healthPoints / _baseStats.GetStat(Stat.Health);
        #endregion

        #region Saving
        public object CaptureState() => _healthPoints;

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;
            if (_healthPoints == 0) Die();
        }
        #endregion
    }
}
