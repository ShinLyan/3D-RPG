using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField, Range(0, 100)] private int _regenerationPercentage;
        private BaseStats _baseStats;

        public float HealthPoints { get; private set; } = -1f;
        public float MaxHealthPoints => _baseStats.GetStat(Stat.Health);

        public float HealthPercentage => 100 * HealthPoints / MaxHealthPoints;
        public bool IsDead { get; private set; }

        private void Start()
        {
            _baseStats = GetComponent<BaseStats>();
            if (_baseStats) _baseStats.OnLevelUp += RegenerateHealth;

            if (HealthPoints < 0) HealthPoints = _baseStats.GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            float regenHP = _baseStats.GetStat(Stat.Health) * _regenerationPercentage / 100;
            HealthPoints = Mathf.Max(HealthPoints, regenHP);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            if (HealthPoints == 0)
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

        #region Saving
        public object CaptureState() => HealthPoints;

        public void RestoreState(object state)
        {
            HealthPoints = (float)state;
            if (HealthPoints == 0) Die();
        }
        #endregion
    }
}
