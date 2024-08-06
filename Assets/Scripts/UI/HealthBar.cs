using RPG.Attributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private HealthBarOwner _owner;
        [SerializeField] private Health _ownerHealth;
        [SerializeField] private Image _backgroundFill;
        [SerializeField] private Image _foregroundFill;
        [SerializeField] private GameObject _bar;
        [SerializeField] private TMP_Text _healthText;

        private Coroutine _currentCoroutine;
        private float _previousValue;

        private enum HealthBarOwner
        {
            Enemy,
            Player
        }

        private void Start()
        {
            UpdateBar();
            if (_owner == HealthBarOwner.Enemy) SwitchBar(false);
        }

        public void UpdateBar()
        {
            if (_owner == HealthBarOwner.Enemy) SwitchBar(true);
            SetBarText(_ownerHealth.HealthPoints, _ownerHealth.MaxHealthPoints);
            SetBarImageFill(_ownerHealth.HealthPoints / _ownerHealth.MaxHealthPoints);
        }

        private void SetBarText(float currentValue, float maxValue)
        {
            _healthText.text = $"HP  {currentValue} / {maxValue}";
        }

        private void SwitchBar(bool enabled) => _bar.SetActive(enabled);

        private void SetBarImageFill(float newValue)
        {
            // _previousValue > newValue :
            // True: Take Damage, False: Heal
            _backgroundFill.color = _previousValue > newValue ? Color.yellow : Color.green;
            Image firstFillBar = _previousValue > newValue ? _foregroundFill : _backgroundFill;
            Image secondFillBar = _previousValue > newValue ? _backgroundFill : _foregroundFill;

            // Первый бар заполняется мгновенно.
            firstFillBar.fillAmount = newValue;

            // Второй бар заполняется плавно.
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(FillBarSmoothlyCoroutine(secondFillBar, newValue));

            _previousValue = newValue;
        }

        private static IEnumerator FillBarSmoothlyCoroutine(Image bar, float value)
        {
            float elapsedTime = 0;
            const float FillingRate = 0.5f;
            while (bar.fillAmount != value)
            {
                elapsedTime += Time.deltaTime * FillingRate;
                bar.fillAmount = Mathf.Lerp(bar.fillAmount, value, elapsedTime);
                yield return null;
            }
            bar.fillAmount = value;
        }

        public void HideHealthBar() => SwitchBar(false);
    }
}
