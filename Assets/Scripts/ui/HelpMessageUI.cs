using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class HelpMessageUI : MonoBehaviour {
        [SerializeField] private Button openHelpButton;
        [SerializeField] private GameObject helpWindow;
        private bool _isHelpWindowOpen;

        private void Awake() {
            _isHelpWindowOpen = false;
            helpWindow.SetActive(false);
        }

        private void Start() {
            openHelpButton.onClick.AddListener(ToggleHelpWindows);
        }

        private void ToggleHelpWindows() {
            _isHelpWindowOpen = !_isHelpWindowOpen;
            helpWindow.SetActive(_isHelpWindowOpen);
        }
    }
}