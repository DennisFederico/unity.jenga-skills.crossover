using UnityEngine;

namespace scriptable {
    [CreateAssetMenu(menuName = "GameConfigSO")]
    public class GameConfigSO : ScriptableObject {
        public DataLoadSource dataLoadSource;
        public string baseUrl;
        public int maxStacks;
    }
    
    public enum DataLoadSource {
        Local,
        Remote
    }
}