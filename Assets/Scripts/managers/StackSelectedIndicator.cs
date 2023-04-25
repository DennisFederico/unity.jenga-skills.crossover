using behaviours.config;
using UnityEngine;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder))]
    public class StackSelectedIndicator : MonoBehaviour {
        
        [SerializeField] private GameObject indicatorPrefab;
        private GameObject _indicator;
        private const float EffectMaxTime = 0.75f;
        private float _effectTime;
        private bool _isEffecting;

        private void Awake() {
            var blockSize = GetComponent<GameConfigHolder>().GameConfig.blockPrefab.GetComponent<BoxCollider>().size;
            var scale = Mathf.Max(blockSize.x,blockSize.z);
            _indicator = Instantiate(indicatorPrefab);
            _indicator.name= "StackSelectedIndicator";
            _indicator.transform.localScale = new Vector2(scale, scale);
            _indicator.SetActive(false);
            GameManager.Instance.OnStackSelectionChange += StartEffect;
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
            GameManager.Instance.OnStackSelectionChange -= StartEffect;
        }

        private void StartEffect(Transform stackCenterMarker) {
            Debug.Log("StartEffect");
            _indicator.SetActive(true);
            PositionIndicator(stackCenterMarker.position);
            _isEffecting = true;
            _effectTime = EffectMaxTime;
        }

        private void StopEffect() {
            _isEffecting = false;
            _indicator.SetActive(false);
        }
        
        private void PositionIndicator(Vector3 markerPosition) {
            var position = new Vector3(markerPosition.x, 0.1f, markerPosition.z);
            _indicator.transform.position = position;
        }
    }
}