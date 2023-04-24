using System.Collections.Generic;
using behaviours;
using config;
using model;
using scriptable;
using UnityEngine;

namespace managers {
    public class GameManager : MonoBehaviour {
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private MasteryMaterialSO masteryMaterialMap;
        [SerializeField] private GameObject stackLabelCanvasPrefab;
        private GameConfigSO _gameConfigSO;
        private Vector3 _blockSize;
        private int _maxStacks;
        private const int BlocksPerRow = 3;
        private float _stacksPadding;
        private float _stackWidth;
        private readonly List<Transform> _stacks = new();
        private readonly List<Transform> _focusPov = new();
        private int _currentStackIndex = 0;

        private float CanvasToWorldScale(float worldWidth, float canvasWidth) => worldWidth / canvasWidth;
        private float StackXOffset(int numStack) => numStack * (_stackWidth + _stacksPadding);
        private readonly Quaternion _rotation90 = Quaternion.Euler(0, 90, 0);

        private void Start() {
            _gameConfigSO = StacksManager.Instance.GetComponent<GameConfigHolder>().GameConfig;
            _maxStacks = _gameConfigSO.maxStacks;
            _blockSize = blockPrefab.GetComponent<BoxCollider>().size;
            _stackWidth = Mathf.Max(_blockSize.x, _blockSize.z);
            _stacksPadding = _stackWidth * 1.5f;
            BuildStacks();
            Debug.Log($"Size: {sizeof(SkillMasteryLevel)}");
        }

        private void BuildStacks() {
            var skillsByGrade = StacksManager.Instance.SortedSkillsByGrade;
            int stackNum = 0;
            foreach (var grade in skillsByGrade.Keys) {
                var stack = new GameObject(grade);
                BuildStackBlocks(stack.transform, skillsByGrade[grade].Values);
                stack.transform.position = new Vector3(StackXOffset(stackNum), 0, 0);
                _stacks.Add(stack.transform);
                CreateStackLabel(stackNum, grade);
                _focusPov.Add(CreateStackFocusPoint(stack.transform, stackNum));

                stackNum++;
                //Max stacks to display reached
                if (stackNum >= _maxStacks) {
                    break;
                }
            }
        }

        private void BuildStackBlocks(Transform stackParent, IList<SkillData> skills) {
            float oddRowZOffset = _blockSize.x;
            float oddRowXOffset = 0;

            int blockCount = 0;
            foreach (var skill in skills) {
                int rowHeight = blockCount / BlocksPerRow;
                int placeInRow = blockCount % BlocksPerRow;
                var block = Instantiate(blockPrefab, stackParent);
                block.name = $"Block_{rowHeight}_{placeInRow}";
                if (rowHeight % 2 == 0) {
                    block.transform.localPosition = new Vector3(placeInRow * _blockSize.x, rowHeight * _blockSize.y, 0);
                } else {
                    block.transform.localRotation = _rotation90;
                    block.transform.localPosition = new Vector3(oddRowXOffset, rowHeight * _blockSize.y, oddRowZOffset + placeInRow * _blockSize.x);
                }

                var jengaBlock = block.GetComponent<JengaBlock>();
                jengaBlock.SetSkillData(skill);
                jengaBlock.SetMaterial(masteryMaterialMap.GetMaterial(skill.mastery));
                blockCount++;
            }
        }

        private Transform CreateStackFocusPoint(Transform stack, int stackNum) {
            var stackCenter = CalculateStackCenter(stack);
            float xOffset = StackXOffset(stackNum);
            float yOffset = _blockSize.y * 0.5f;
            var stackPov = new GameObject($"POV_{stackNum}") {
                transform = {
                    position = stackCenter + (Vector3.right * xOffset) + (Vector3.up * yOffset)
                }
            };
            return stackPov.transform;
        }

        private Vector3 CalculateStackCenter(Transform stack) {
            int numBlocks = stack.childCount;
            int numRows = numBlocks / BlocksPerRow;
            return new Vector3(_blockSize.x * BlocksPerRow * 0.5f, _blockSize.y * numRows * 0.5f, _blockSize.z * 0.5f);
        }

        private void CreateStackLabel(int stackNum, string grade) {
            var stackLabelCanvas = Instantiate(stackLabelCanvasPrefab);
            stackLabelCanvas.name = $"StackLabelCanvas_{stackNum}";
            stackLabelCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = grade;

            var canvasRect = stackLabelCanvas.GetComponent<RectTransform>();
            var scale = _stackWidth * 1.5f / canvasRect.rect.width;
            stackLabelCanvas.transform.position = new Vector3(_stackWidth * 0.5f + StackXOffset(stackNum), canvasRect.rect.height * 0.5f * scale, -2);
            stackLabelCanvas.transform.localScale = Vector3.one * scale;
        }

        private void ResizeWorldCanvas(RectTransform canvas, float expectedWidth) {
            var scale = expectedWidth / canvas.rect.width;
            canvas.transform.localScale = Vector3.one * scale;
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