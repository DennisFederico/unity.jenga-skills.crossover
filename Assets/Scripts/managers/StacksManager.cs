using System;
using System.Collections;
using System.Collections.Generic;
using behaviours.block;
using behaviours.config;
using model;
using scriptable;
using ui;
using UnityEngine;
using utils;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder))]
    public class StacksManager : MonoBehaviour {
        public static StacksManager Instance { get; private set; }

        public event Action OnStacksBuilt;

        private GameConfigSO _gameConfig;

        //Cache variables to avoid repeated calls to GetComponent or other calculations
        private Vector3 _blockSize;
        private float _stacksPadding;
        private float _stackWidth;

        private readonly Dictionary<int,Transform> _stacks = new();
        private readonly List<Transform> _focusPov = new();
        private readonly Dictionary<string, int> _gradeStackIndexMap = new();
        private readonly Dictionary<int,string> _stackIndexGradeMap = new();

        private float StackXOffset(int numStack) => numStack * (_stackWidth + _stacksPadding);
        private readonly Quaternion _rotation90 = Quaternion.Euler(0, 90, 0);

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            _gameConfig = GetComponent<GameConfigHolder>().GameConfig;
            _blockSize = _gameConfig.blockPrefab.GetComponent<BoxCollider>().size;
            _stackWidth = Mathf.Max(_blockSize.x, _blockSize.z);
            _stacksPadding = _stackWidth * 1.5f;
            BuildAllStacks();
            OnStacksBuilt?.Invoke();
        }

        private void BuildAllStacks() {
            var skillsByGrade = StacksDataLoader.Instance.SortedSkillsByGrade;
            int stackNum = 0;
            
            //First build the Stacks
            foreach (var grade in skillsByGrade.Keys) {
                BuildSingleStack(stackNum, grade, skillsByGrade[grade].Values);
                _gradeStackIndexMap.Add(grade, stackNum);
                _stackIndexGradeMap.Add(stackNum, grade);
                stackNum++;
                //Max stacks to display reached
                if (stackNum >= _gameConfig.maxStacks) {
                    break;
                }
            }
            
            //Now build the labels and camera focus points
            for (var stackIndex = 0; stackIndex < _stacks.Count; stackIndex++) {
                var skillsCount = skillsByGrade[_stackIndexGradeMap[stackIndex]].Values.Count;
                _focusPov.Add(CreateStackFocusPoint(stackIndex, skillsCount));
                CreateStackLabel(stackIndex, _stackIndexGradeMap[stackIndex]);
            }
        }

        private void BuildSingleStack(int stackNum, string grade, IList<SkillData> skills) {
            var stack = new GameObject(grade);
            stack.transform.position = new Vector3(StackXOffset(stackNum), 0, 0);
            _stacks.Add(stackNum, stack.transform);
            //BuildStackBlocks(stack.transform, skills);
            StartCoroutine(BuildStackBlocks(stack.transform, skills));
        }

        private IEnumerator BuildStackBlocks(Transform stackParent, IList<SkillData> skills) {
            float oddRowZOffset = _blockSize.x;
            float oddRowXOffset = 0;

            int blockCount = 0;
            foreach (var skill in skills) {
                int rowHeight = blockCount / _gameConfig.blocksPerRow;
                int placeInRow = blockCount % _gameConfig.blocksPerRow;
                var block = Instantiate(_gameConfig.blockPrefab, stackParent);
                block.name = $"Block_{rowHeight}_{placeInRow}";
                if (rowHeight % 2 == 0) {
                    block.transform.localPosition = new Vector3(placeInRow * _blockSize.x, rowHeight * _blockSize.y, 0);
                } else {
                    block.transform.localRotation = _rotation90;
                    block.transform.localPosition = new Vector3(oddRowXOffset, rowHeight * _blockSize.y, oddRowZOffset + placeInRow * _blockSize.x);
                }

                block.GetComponent<BlockSkillDataHolder>().SetSkillData(skill);
                block.GetComponent<BlockMaterialHandler>().SetMaterial(_gameConfig.masteryMaterialMap.GetMaterial(skill.mastery));
                blockCount++;
                yield return new WaitForSeconds(_gameConfig.blockPlacementSpeed);
            }
        }

        private void CreateStackLabel(int stackNum, string grade) {
            var stackLabelCanvas = Instantiate(_gameConfig.stackLabelCanvasPrefab);
            stackLabelCanvas.name = $"StackLabelCanvas_{stackNum}";
            stackLabelCanvas.GetComponent<StackLabelUI>().SetGradeAndIndex(grade, stackNum);

            var canvasRect = stackLabelCanvas.GetComponent<RectTransform>();
            var scale = Utils.CanvasToWorldScale(_stackWidth * 1.5f, canvasRect.rect.width);
            stackLabelCanvas.transform.position = new Vector3(_stackWidth * 0.5f + StackXOffset(stackNum), canvasRect.rect.height * 0.5f * scale, -2);
            stackLabelCanvas.transform.localScale = Vector3.one * scale;
        }

        private Transform CreateStackFocusPoint(int stackNum, int numBlocks) {
            var stackCenter = CalculateStackCenter(numBlocks);
            float xOffset = StackXOffset(stackNum);
            float yOffset = _blockSize.y * 0.5f;
            var stackPov = new GameObject($"POV_{stackNum}") {
                transform = {
                    position = stackCenter + (Vector3.right * xOffset) + (Vector3.up * yOffset)
                }
            };
            return stackPov.transform;
        }

        private Vector3 CalculateStackCenter(int numBlocks) {
            int numRows = numBlocks / _gameConfig.blocksPerRow;
            return new Vector3(_blockSize.x * _gameConfig.blocksPerRow * 0.5f, _blockSize.y * numRows * 0.5f, _blockSize.z * 0.5f);
        }

        public int GetNumStacks() => _stacks.Count;

        public Transform GetStackFocusPoint(int stackNum) => _focusPov[stackNum];

        public Transform GetStack(int stackIndex) {
            return _stacks[stackIndex];
        }
        
        public Transform GetStack(string grade) {
            return _stacks[_gradeStackIndexMap[grade]];
        }
        
        public void RebuildStack(int numStack) {
            if (_stacks.TryGetValue(numStack, out var stack)) {
                Destroy(stack.gameObject);
                _stacks.Remove(numStack);
            }
            var grade = _stackIndexGradeMap[numStack];
            BuildSingleStack(numStack, grade, StacksDataLoader.Instance.SortedSkillsByGrade[grade].Values);
        }
    }
}