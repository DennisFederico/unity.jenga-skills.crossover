using UnityEngine;
using UnityEngine.SceneManagement;

namespace managers {
    public class SceneLoadManager : MonoBehaviour {
        public enum Scene {
            LoadingScene,
            GameScene,
        }

        public static SceneLoadManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(Scene scene) {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}