using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "Save";
        private const float FadeInTime = 0.2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
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
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public static void Delete()
        {
            SavingSystem.Delete(DefaultSaveFile);
            print("Delete Save File");
        }
    }
}
