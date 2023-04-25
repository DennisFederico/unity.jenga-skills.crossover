using scriptable;
using UnityEngine;

namespace behaviours.config {
    public class GameConfigHolder : MonoBehaviour {
        [SerializeField] private GameConfigSO gameConfig;
        public GameConfigSO GameConfig => gameConfig;
    }
}