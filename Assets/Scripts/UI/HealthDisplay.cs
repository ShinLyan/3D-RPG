using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _healthText.text = $"Health: {_health.HealthPoints}/{_health.MaxHealthPoints}";
        }
    }
}
