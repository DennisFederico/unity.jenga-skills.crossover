using Cinemachine;
using UnityEngine;

namespace managers {
    public class CameraController : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineFramingTransposer _transposer;
        private const float StartingCameraDistance = 25;
        private readonly (float min, float max) _cameraDistanceRange = (15f, 35f);
        private readonly float _scrollSpeed = 3f;
        private int _totalStacks;

        private void Awake() {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _transposer.m_CameraDistance = StartingCameraDistance*2;
        }

        private void Start() {
            GameManager.Instance.OnStackFocusChange += SetCameraTarget;
        }

        private void LateUpdate() {
            if (Input.GetMouseButton(1)) {
                HandleCameraRotation();
            }
            
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput == 0f) return;
            
            float newDistance = _transposer.m_CameraDistance - scrollInput * _scrollSpeed;
            //newDistance = Mathf.Clamp(newDistance, _cameraDistanceRange.min, _cameraDistanceRange.max);
            _transposer.m_CameraDistance = Mathf.Clamp(newDistance, _cameraDistanceRange.min, _cameraDistanceRange.max);
        }

        private void OnDestroy() {
            GameManager.Instance.OnStackFocusChange -= SetCameraTarget;
        }

        private void HandleCameraRotation() {
            //if (!_currentTarget) return;
            var xAxis = Input.GetAxis("Mouse X");
            var yAxis = Input.GetAxis("Mouse Y");
            var rotation = virtualCamera.transform.localEulerAngles;
            rotation.y += xAxis;
            rotation.x -= yAxis;
            virtualCamera.transform.localEulerAngles = rotation;
        }

        private void SetCameraTarget(Transform target) {
            virtualCamera.Follow = target;
        }
    }
}