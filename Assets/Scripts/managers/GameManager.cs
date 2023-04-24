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
        private GameConfigSO _gameConfigSO;
        private Vector3 _blockSize;
        private int _maxStacks;
        private int _blocksPerRow = 3;
        private float _stacksPadding;
        private float _stackWidth;
        private List<Transform> _stacks = new();
        private List<Transform> _focusPov = new();
        private int _currentStackIndex = 0;

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
                var stack = BuildStack(grade, skillsByGrade[grade].Values);
                _stacks.Add(stack);

                //Focus position
                var stackCenter = CalculateStackCenter(stack);

                //Position stack and POV
                float xOffset = stackNum * (_stackWidth + _stacksPadding);
                stack.position = new Vector3(xOffset, 0, 0);
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = stackCenter + (Vector3.right * xOffset);
                sphere.name = $"POV_{grade}";
                _focusPov.Add(sphere.transform);

                stackNum++;
                //Max stacks to display reached
                if (stackNum >= _maxStacks) {
                    break;
                }
            }
        }

        private Transform BuildStack(string label, IList<SkillData> skills) {
            var stack = new GameObject(label);
            BuildStackBlocks(stack.transform, skills);
            return stack.transform;
        }

        private void BuildStackBlocks(Transform stackParent, IList<SkillData> skills) {
            float oddRowZOffset = _blockSize.x;
            float oddRowXOffset = 0;

            int blockCount = 0;
            foreach (var skill in skills) {
                int rowHeight = blockCount / _blocksPerRow;
                int placeInRow = blockCount % _blocksPerRow;
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

        private Vector3 CalculateStackCenter(Transform stack) {
            int numBlocks = stack.childCount;
            int numRows = numBlocks / _blocksPerRow;
            return new Vector3(_blockSize.x * _blocksPerRow / 2, _blockSize.y * numRows / 2, _blockSize.z / 2);
        }
    }
}