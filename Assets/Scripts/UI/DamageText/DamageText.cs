using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _damageText;

        public void SetTextValue(float value)
        {
            _damageText.text = $"{value}"; 
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
