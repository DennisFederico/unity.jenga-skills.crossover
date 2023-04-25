using System;
using Cinemachine;
using UnityEngine;

namespace managers {
    public class CameraController : MonoBehaviour {
        public static CameraController Instance { get; private set; }
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineFramingTransposer _transposer;
        private float _startingCameraDistance = 25;
        private readonly (float min, float max) _cameraDistanceRange = (15f, 35f);
        private readonly float _scrollSpeed = 3f;
        private Transform _currentTarget;
        private int _totalStacks;
        private int _currentStackPovIndex = 0;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _transposer.m_CameraDistance = _startingCameraDistance*2;
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
        
        private void HandleCameraRotation() {
            //if (!_currentTarget) return;
            var xAxis = Input.GetAxis("Mouse X");
            var yAxis = Input.GetAxis("Mouse Y");
            var rotation = virtualCamera.transform.localEulerAngles;
            rotation.y += xAxis;
            rotation.x -= yAxis;
            virtualCamera.transform.localEulerAngles = rotation;
        }

        public void SetCameraTarget(Transform target) {
            _currentTarget = target;
            virtualCamera.Follow = target;
        }
    }
}