using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "Save";
        private const float FadeInTime = 0.5f;

        private IEnumerator Start()
        {
            Fader.Instance.FadeOutImmediate();
            yield return SavingSystem.LoadLastScene(DefaultSaveFile);
            yield return Fader.Instance.FadeIn(FadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public static void Load()
        {
            SavingSystem.Load(DefaultSaveFile);
        }

        public static void Save()
        {
            SavingSystem.Save(DefaultSaveFile);
        }
    }
}
