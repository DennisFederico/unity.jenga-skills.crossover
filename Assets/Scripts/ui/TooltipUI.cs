using TMPro;
using UnityEngine;

namespace ui {
    public class TooltipUI : MonoBehaviour {
        public static TooltipUI Instance { get; private set; }

        [SerializeField] private RectTransform parentCanvas;
        [SerializeField] private TextMeshProUGUI messageText;
        private RectTransform _rectTransform;
        private Vector3 _pointerOffset;
        private float _width;
        private float _height;
        private TooltipTimer _timer;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            _rectTransform = GetComponent<RectTransform>();
            _pointerOffset = new Vector2(_rectTransform.rect.width * 0.5f, -_rectTransform.rect.height * 0.5f);
            Hide();
        }

        private void LateUpdate() {
            HandleTooltipPosition();
            if (_timer == null) return;
            _timer.Update();
            if (_timer.IsFinished()) {
                Hide();
            }
        }

        private void HandleTooltipPosition() {
            //ASSUME RECT PIVOTED ON (0,0) - BOTTOM LEFT
            var position = Input.mousePosition / parentCanvas.localScale.x + _pointerOffset;
            position.x = Mathf.Min(position.x, parentCanvas.rect.width - _rectTransform.rect.width * 0.5f);
            position.x = Mathf.Max(position.x, _rectTransform.rect.width * 0.5f);
            position.y = Mathf.Min(position.y, parentCanvas.rect.height - _rectTransform.rect.height * 0.5f);
            position.y = Mathf.Max(position.y, _rectTransform.rect.height * 0.5f);
            _rectTransform.anchoredPosition = position;
        }

        public void Show(string message, float timer = 0) {
            _timer = timer > 0 ? new TooltipTimer(timer) : null;
            messageText.text = message;
            messageText.ForceMeshUpdate(true);
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public class TooltipTimer {
            private float _duration;

            public TooltipTimer(float duration) {
                _duration = duration;
            }

            public void Update() {
                _duration -= Time.deltaTime;
            }

            public bool IsFinished() {
                return _duration < 0;
            }
        }
    }
}