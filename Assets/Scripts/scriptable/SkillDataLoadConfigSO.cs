using UnityEngine;

namespace scriptable {
    [CreateAssetMenu(menuName = "SkillDataLoadConfigSO")]
    public class SkillDataLoadConfigSO : ScriptableObject {
        public DataLoadSource dataLoadSource;
        public string baseUrl;
    }
    
    public enum DataLoadSource {
        Local,
        Remote,
        WebGL
    }
}