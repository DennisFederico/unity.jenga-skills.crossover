using UnityEngine;

namespace scriptable {
    [CreateAssetMenu(menuName = "GameConfigSO")]
    public class GameConfigSO : ScriptableObject {
        public GameObject blockPrefab;
        public GameObject stackLabelCanvasPrefab;
        public MasteryMaterialSO masteryMaterialMap;
        public int maxStacks = 3;
        public int blocksPerRow = 3;
    }
}