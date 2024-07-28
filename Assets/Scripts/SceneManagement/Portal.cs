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

        private const float FadeOutTime = 0.5f;
        private const float FadeInTime = 2f;
        private const float FadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            SwitchPlayerMovement(false); // Чтобы персонаж не двигался до загрузки другой сцены

            yield return Fader.Instance.FadeOut(FadeOutTime);

            SavingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            SavingWrapper.Load();

            UpdatePlayerPosition(GetOtherPortal());
            SwitchPlayerMovement(false); // Чтобы персонаж не двигался после загрузки сцены

            SavingWrapper.Save();

            yield return new WaitForSeconds(FadeWaitTime);
            Fader.Instance.FadeIn(FadeInTime);

            SwitchPlayerMovement(true);

            Destroy(gameObject);
        }

        private void SwitchPlayerMovement(bool enabled)
        {
            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = enabled;
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
