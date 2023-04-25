using System;
using behaviours.config;
using scriptable;
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
        
        
        

        // private void Update() {
        //     if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        //         _currentStackIndex--;
        //         if (_currentStackIndex < 0) {
        //             _currentStackIndex = _stacks.Count - 1;
        //         }
        //     } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        //         _currentStackIndex++;
        //         if (_currentStackIndex >= _stacks.Count) {
        //             _currentStackIndex = 0;
        //         }
        //     }
        //
        //     for (int i = 0; i < _stacks.Count; i++) {
        //         var stack = _stacks[i];
        //         var stackPov = _focusPov[i];
        //         if (i == _currentStackIndex) {
        //             stackPov.gameObject.SetActive(true);
        //             stack.localScale = Vector3.one;
        //         } else {
        //             stackPov.gameObject.SetActive(false);
        //             stack.localScale = Vector3.one * 0.5f;
        //         }
        //     }
        // }
    }
}