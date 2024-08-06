using UnityEngine;

namespace RPG.UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _pauseActive;

        private bool PauseActive
        {
            get => _pauseActive;
            set
            {
                _pauseActive = value;
                SwitchChildren(_pauseActive);
            }
        }

        private void Start()
        {
            PauseActive = false;
            SwitchChildren(PauseActive);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                PauseActive = !PauseActive;
            }
        }

        private void SwitchChildren(bool enabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }
}
