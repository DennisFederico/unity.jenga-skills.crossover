using managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class StackLabelUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private Button testStackButton;
        [SerializeField] private Button resetStackButton;
        private bool _tested;
        private string _grade;

        private void Awake() {
            testStackButton.onClick.AddListener(() => {
                _tested = true;
                GameManager.Instance.TestStack(_grade);
                ToggleButtons();
            });
            
            resetStackButton.onClick.AddListener(() => {
                _tested = false;
                Debug.Log($"Reset {_grade}");
                ToggleButtons();
            });
            ToggleButtons();
        }

        private void ToggleButtons() {
            testStackButton.gameObject.SetActive(!_tested);
            resetStackButton.gameObject.SetActive(_tested);
        }
        
        public void SetGrade(string grade) {
            _grade = grade;
            gradeText.text = grade;
        }
    }
}