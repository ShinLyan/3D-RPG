using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");

            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            _player = GameObject.FindWithTag("Player");
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
