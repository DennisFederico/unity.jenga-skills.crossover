using System;
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
        private int _stackIndex;
        private string _grade;

        private void Awake() {
            testStackButton.onClick.AddListener(() => {
                _tested = true;
                GameManager.Instance.TestStack(_grade);
                ToggleButtons();
            });
            
            resetStackButton.onClick.AddListener(() => {
                _tested = false;
                GameManager.Instance.RebuildStack(_stackIndex);
                ToggleButtons();
            });
            ToggleButtons();
        }

        private void Start() {
            GameManager.Instance.OnSelectedStackAction += index => {
                if (index == _stackIndex) {
                    PerformAvailableAction();
                }
            };
        }

        private void ToggleButtons() {
            testStackButton.gameObject.SetActive(!_tested);
            resetStackButton.gameObject.SetActive(_tested);
        }

        private void PerformAvailableAction() {
            if (_tested) {
                resetStackButton.onClick.Invoke();
            } else {
                testStackButton.onClick.Invoke();
            }
        }
        
        public void SetGradeAndIndex(string grade, int stackIndex) {
            _stackIndex = stackIndex;
            _grade = grade;
            gradeText.text = grade;
        }
    }
}