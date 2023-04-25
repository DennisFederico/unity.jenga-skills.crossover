using behaviours.config;
using UnityEngine;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder))]
    public class StackSelectedIndicator : MonoBehaviour {
        
        [SerializeField] private GameObject indicator;
        private float _indicatorScale;
        private const float EffectMaxTime = 0.75f;
        private float _effectTime;
        private bool _isEffecting;

        private void Awake() {
            var blockSize = GetComponent<GameConfigHolder>().GameConfig.blockPrefab.GetComponent<BoxCollider>().size;
            _indicatorScale = Mathf.Max(blockSize.x,blockSize.z) * 1.5f;
            indicator.transform.localScale = new Vector2(_indicatorScale, _indicatorScale);
            indicator.SetActive(false);
        }

        private void Start() {
            GameManager.Instance.OnStackFocusChange += StartEffect;
        }

        private void Update() {
            if (_isEffecting) {

                _effectTime -= Time.deltaTime;
                if (_effectTime <= 0) {
                    StopEffect();
                }
            }
        }

        private void OnDestroy() {
            GameManager.Instance.OnStackFocusChange -= StartEffect;
        }

        private void StartEffect(Transform stackCenterMarker) {
            PositionIndicator(stackCenterMarker.position);
            _isEffecting = true;
            _effectTime = EffectMaxTime;
            indicator.SetActive(true);
        }

        private void StopEffect() {
            _isEffecting = false;
            indicator.SetActive(false);
        }
        
        private void PositionIndicator(Vector3 markerPosition) {
            var position = new Vector3(markerPosition.x, 0.1f, markerPosition.z);
            indicator.transform.position = position;
        }
    }
}