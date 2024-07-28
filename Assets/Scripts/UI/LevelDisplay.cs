using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;

        private BaseStats _baseStats;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _levelText.text = $"Level: {_baseStats.CurrentLevel}";
        }
    }
}