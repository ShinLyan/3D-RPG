using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _experienceText;

        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _experienceText.text = $"XP: {(int)_experience.ExperiencePoints}";
        }
    }
}