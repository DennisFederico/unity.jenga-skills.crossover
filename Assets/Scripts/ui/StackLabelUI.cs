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
        private bool _actionsEnabled;
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
            HideButtons();
        }

        private void Start() {
            StacksManager.Instance.OnStacksBuilt += () => {
                EnableActions();
                ToggleButtons();
            };
            
            StacksManager.Instance.OnStackBuilding += (index) => {
                if (index == _stackIndex) {
                    HideButtons();
                    DisableActions();
                }
            };
            
            StacksManager.Instance.OnStackBuilt += (index) => {
                if (index == _stackIndex) {
                    EnableActions();
                    ToggleButtons();
                }
            };
        }

        private void EnableActions() {
            GameManager.Instance.OnSelectedStackAction += PerformAvailableAction;
            _actionsEnabled = true;
        }

        private void DisableActions() {
            GameManager.Instance.OnSelectedStackAction -= PerformAvailableAction;
            _actionsEnabled = false;
        }
        
        private void PerformAvailableAction(int index) {
            if (index == _stackIndex) {
                if (_tested) {
                    resetStackButton.onClick.Invoke();
                } else {
                    testStackButton.onClick.Invoke();
                }
            }
        }

        private void HideButtons() {
            testStackButton.gameObject.SetActive(false);
            resetStackButton.gameObject.SetActive(false);
        }
        
        private void ToggleButtons() {
            if (!_actionsEnabled) return;
            testStackButton.gameObject.SetActive(!_tested);
            resetStackButton.gameObject.SetActive(_tested);
        }

        public void SetGradeAndIndex(string grade, int stackIndex) {
            _stackIndex = stackIndex;
            _grade = grade;
            gradeText.text = grade;
        }
    }
}