using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField, Range(1, 99)] private int _startingLevel = 1;
        [SerializeField] private Progression _progression;
        [SerializeField] private DestroyAfterEffect _levelUpParticleEffect;
        [SerializeField] private bool _shouldUseModifiers;
        private int _currentLevel;

        public event Action OnLevelUp;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if (experience) experience.OnExperienceGained += UpdateLevel;
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
                OnLevelUp();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) => !_shouldUseModifiers ? GetBaseStat(stat) :
            (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);

        private float GetBaseStat(Stat stat) => _progression.GetStat(stat, _characterClass, GetLevel());

        private float GetAdditiveModifier(Stat stat)
        {
            float totalValue = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    totalValue += modifier;
                }
            }
            return totalValue;
        }

        private float GetPercentageModifier(Stat stat)
        {
            float totalPercentage = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    totalPercentage += modifier;
                }
            }
            return totalPercentage;
        }

        public int GetLevel() => _currentLevel < 1 ? CalculateLevel() : _currentLevel;
    }
}
