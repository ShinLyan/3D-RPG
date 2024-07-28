using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _enemyHealthText;

        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            _enemyHealthText.text = _fighter.Target ?
                $"Enemy: {_fighter.Target.HealthPoints}/{_fighter.Target.MaxHealthPoints}" : $"N/A";
        }
    }
}
