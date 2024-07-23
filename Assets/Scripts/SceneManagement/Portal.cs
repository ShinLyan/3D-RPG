using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int _sceneToLoad;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;

        [Header("FadeOutIn")]
        [SerializeField] private float _fadeOutTime = 0.5f;
        [SerializeField] private float _fadeInTime = 1f;
        [SerializeField] private float _fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            SwitchPlayerMovement(false); // Чтобы персонаж не двигался до загрузки другой сцены

            yield return Fader.Instance.FadeOut(_fadeOutTime);

            SavingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            SavingWrapper.Load();

            UpdatePlayerPosition(GetOtherPortal());
            SwitchPlayerMovement(false); // Чтобы персонаж не двигался после загрузки сцены

            SavingWrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return Fader.Instance.FadeIn(_fadeInTime);

            SwitchPlayerMovement(true);

            Destroy(gameObject);
        }

        private void SwitchPlayerMovement(bool enabled)
        {
            var _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _playerController.enabled = enabled;
        }

        private void UpdatePlayerPosition(Portal portal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal._spawnPoint.position);
            player.transform.rotation = portal._spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this || portal._destination != _destination) continue;
                return portal;
            }
            return null;
        }
    }
}
