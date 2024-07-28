using RPG.Attributes;
using UnityEngine;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _healthComponent;
        [SerializeField] private RectTransform _foreground;
        [SerializeField] private Canvas _canvas;

        private void Update()
        {
            if (_healthComponent.IsDead) Destroy(gameObject);

            float healthFraction = _healthComponent.HealthPoints / _healthComponent.MaxHealthPoints;
            if (Mathf.Approximately(healthFraction, 1))
            {
                SwitchBar(false);
                return;
            }

            SwitchBar(true);
            _foreground.localScale = new Vector3(healthFraction, 1f);
        }

        private void SwitchBar(bool enabled)
        {
            _canvas.enabled = enabled;
        }
    }
}
