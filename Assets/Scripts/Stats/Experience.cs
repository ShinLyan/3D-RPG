using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    [RequireComponent(typeof(BaseStats))]
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;

        public float ExperiencePoints => _experiencePoints;

        public event System.Action OnExperienceGained;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained();
        }

        #region Saving
        public object CaptureState() => _experiencePoints;

        public void RestoreState(object state) => _experiencePoints = (float)state;
        #endregion
    }
}
