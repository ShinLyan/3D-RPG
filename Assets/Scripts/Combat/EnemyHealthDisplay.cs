using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _enemyHealthText; // HACK: удалить класс, соединить с HEALTHDISPLAY

        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            _enemyHealthText.text = _fighter.Target ?
                $"Enemy: {(int)_fighter.Target.GetPercentage()}%" : $"N/A";
        }
    }
}
