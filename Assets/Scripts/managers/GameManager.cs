using System;
using behaviours;
using behaviours.config;
using model;
using scriptable;
using Unity.VisualScripting;
using UnityEngine;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder), typeof(StacksManager))]
    public class GameManager : MonoBehaviour {
        
        private GameConfigSO _gameConfig;
        private int _totalStacks;
        private int _currentStackIndex;

        private void Start() {
            StacksManager.Instance.OnStacksBuilt += () => {
                _totalStacks = StacksManager.Instance.GetNumStacks();
                CameraController.Instance.SetCameraTarget(StacksManager.Instance.GetStackFocusPoint(_currentStackIndex));
            };
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                TestSelectedStack(_currentStackIndex);
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
        
        private void TestSelectedStack(int stackIndex, SkillMasteryLevel masteryLevel = SkillMasteryLevel.Glass) {
            var stack = StacksManager.Instance.GetStack(stackIndex);
            foreach (Transform child in stack) {
                if (child.TryGetComponent(out JengaBlock jengaBlock) && jengaBlock.GetMasteryLevel() == masteryLevel) {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}