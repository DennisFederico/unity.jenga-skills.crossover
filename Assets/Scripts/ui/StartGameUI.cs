using managers;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class StartGameUI : MonoBehaviour {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button quitGameButton;
        [SerializeField] private GameObject loadingMessageContainer;
        
        private void Awake() {
            startGameButton.onClick.AddListener(StartGame);
            quitGameButton.onClick.AddListener(Application.Quit);
            startGameButton.interactable = false;
        }

        private void Start() {
            StacksDataLoader.Instance.OnStacksLoaded += EnableStartGame;
        }

        private void OnDestroy() {
            StacksDataLoader.Instance.OnStacksLoaded -= EnableStartGame;
        }

        private void EnableStartGame() {
            startGameButton.interactable = true;
            loadingMessageContainer.SetActive(false);
        }

        private void StartGame() {
            SceneLoadManager.Instance.LoadScene(SceneLoadManager.Scene.GameScene);
        }
    }
}