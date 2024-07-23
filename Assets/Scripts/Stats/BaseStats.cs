using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField, Range(1, 10)] private int _startingLevel = 1;
        [SerializeField] private Progression _progression;
        private int _currentLevel;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if (experience)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private int CalculateLevel()
        {
            var experience = GetComponent<Experience>();
            if (!experience) return _startingLevel;

            float currentXP = experience.ExperiencePoints;
            int penultimateLevel = _progression.GetLevelsCount(Stat.ExperienceToLevelUp, _characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if (XPToLevelUp > currentXP) return level;
            }
            return penultimateLevel + 1;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                print("Level Up!");
            }
        }

        public float GetStat(Stat stat) =>
            _progression.GetStat(stat, _characterClass, GetLevel());

        public int GetLevel() => _currentLevel < 1 ? CalculateLevel() : _currentLevel;
    }
}
