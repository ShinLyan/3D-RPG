using RPG.Attributes;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;

        public void SpawnDamageText(float damageAmount)
        {
            DamageText instance = Instantiate(_damageTextPrefab, transform);
            instance.SetTextValue(damageAmount);
        }
    }
}
