using System;
using behaviours.block;
using behaviours.config;
using model;
using scriptable;
using UnityEngine;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder), typeof(StacksManager))]
    public class GameManager : MonoBehaviour {
        
        public static GameManager Instance { get; private set; }
        
        private GameConfigSO _gameConfig;
        private int _totalStacks;
        private int _currentStackIndex;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            StacksManager.Instance.OnStacksBuilt += () => {
                _totalStacks = StacksManager.Instance.GetNumStacks();
                CameraController.Instance.SetCameraTarget(StacksManager.Instance.GetStackFocusPoint(_currentStackIndex));
            };
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                TestStack(_currentStackIndex);
            }
        }

        private void LateUpdate() {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                SwitchPov(_currentStackIndex++);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                SwitchPov(_currentStackIndex--);
            }
        }
        
        private void SwitchPov(int stackIndex) {
            _currentStackIndex = Mathf.Clamp(_currentStackIndex, 0, _totalStacks - 1);
            CameraController.Instance.SetCameraTarget(StacksManager.Instance.GetStackFocusPoint(_currentStackIndex));
        }
        
        public void TestStack(string grade, SkillMasteryLevel masteryLevel = SkillMasteryLevel.Glass) {
            var stack = StacksManager.Instance.GetStack(grade);
            TestSelectedStack(stack, masteryLevel);
        }
        
        public void TestStack(int stackIndex, SkillMasteryLevel masteryLevel = SkillMasteryLevel.Glass) {
            var stack = StacksManager.Instance.GetStack(stackIndex);
            TestSelectedStack(stack, masteryLevel);
        }

        private void TestSelectedStack(Transform stack, SkillMasteryLevel masteryLevel) {
            foreach (Transform child in stack) {
                if (child.TryGetComponent(out BlockSkillDataHolder jengaBlock) && jengaBlock.GetMasteryLevel() == masteryLevel) {
                    Destroy(child.gameObject);
                }
            }
        }

        public void ExitGame() {
            Application.Quit();
        }
    }
}