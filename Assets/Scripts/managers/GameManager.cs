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
        
        //Action with the center of the stack as parameter
        public event Action<Transform> OnStackSelectionChange;
        public event Action<int> OnSelectedStackAction;
        private GameConfigSO _gameConfig;
        private int _totalStacks;
        private int _currentStackIndex;
        private bool _gameStarted;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            StacksManager.Instance.OnStacksBuilding += () => {
                _totalStacks = StacksManager.Instance.GetNumStacks();
                _currentStackIndex = Mathf.FloorToInt(_totalStacks * 0.5f);
                OnStackSelectionChange?.Invoke(StacksManager.Instance.GetStackFocusPoint(_currentStackIndex));
            };
            
            StacksManager.Instance.OnStacksBuilt += () => {
                _gameStarted = true;
            };
        }

        private void Update() {
            if (!_gameStarted) return;
            if (Input.GetKeyDown(KeyCode.Space)) {
                OnSelectedStackAction?.Invoke(_currentStackIndex);
                //TestStack(_currentStackIndex);
            }
        }

        private void LateUpdate() {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                SwitchPov(_currentStackIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                SwitchPov(_currentStackIndex- 1);
            }
        }
        
        private void SwitchPov(int stackIndex) {
            var newIndex = Mathf.Clamp(stackIndex, 0, _totalStacks - 1);
            if (newIndex == _currentStackIndex) return;
            _currentStackIndex = newIndex;
            OnStackSelectionChange?.Invoke(StacksManager.Instance.GetStackFocusPoint(_currentStackIndex));
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
        
        public void RebuildStack(int stackIndex) {
            StacksManager.Instance.RebuildStackAsync(stackIndex);
        }

        public void ExitGame() {
            //Application.Quit();
            SceneLoadManager.Instance.LoadScene(SceneLoadManager.Scene.LoadingScene);
        }
    }
}