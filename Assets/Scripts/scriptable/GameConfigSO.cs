using UnityEngine;

namespace scriptable {
    [CreateAssetMenu(menuName = "GameConfigSO")]
    public class GameConfigSO : ScriptableObject {
        public GameObject blockPrefab;
        public GameObject stackLabelCanvasPrefab;
        public MasteryMaterialSO masteryMaterialMap;
        public float blockPlacementSpeed = 0.1f; //100ms 
        public int maxStacks = 3;
        public int blocksPerRow = 3;
    }
}