using UnityEngine;

namespace scriptable {
    [CreateAssetMenu(menuName = "GameConfig")]
    public class GameConfigSO : ScriptableObject {
        [SerializeField] private string baseUrl;

        public string BaseUrl => baseUrl;
    }
}