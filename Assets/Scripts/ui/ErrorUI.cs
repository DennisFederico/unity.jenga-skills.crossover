using managers;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class ErrorUI : MonoBehaviour {
        [SerializeField] private GameObject container;
        [SerializeField] private TMPro.TextMeshProUGUI errorText;
        [SerializeField] private Button exitButton;

        private void Awake() {
            exitButton.onClick.AddListener(() => Application.Quit());
            Hide();
        }

        private void Start() {
            StacksDataLoader.Instance.OnAnyError += Show;
        }

        private void OnDestroy() {
            StacksDataLoader.Instance.OnAnyError -= Show;
        }

        public void Show(string message) {
            errorText.text = message;
            container.SetActive(true);
        }

        private void Hide() {
            container.SetActive(false);
        }
    }
}