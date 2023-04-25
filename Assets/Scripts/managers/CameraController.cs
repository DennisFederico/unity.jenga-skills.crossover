using System;
using Cinemachine;
using UnityEngine;

namespace managers {
    public class CameraController : MonoBehaviour {
        public static CameraController Instance { get; private set; }
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private Transform _currentTarget;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void LateUpdate() {
            if (Input.GetMouseButton(1)) {
                HandleCameraRotation();
            }
        }

        public void SetTarget(Transform target) {
            _currentTarget = target;
            virtualCamera.Follow = target;
        }
        
        private void HandleCameraRotation() {
            //if (!_currentTarget) return;
            var xAxis = Input.GetAxis("Mouse X");
            var yAxis = Input.GetAxis("Mouse Y");
            var rotation = virtualCamera.transform.localEulerAngles;
            rotation.y += xAxis;
            rotation.x -= yAxis;
            virtualCamera.transform.localEulerAngles = rotation;
            
            //virtualCameraObject.transform.localEulerAngles to rotate the camera around the target.
            //Then just tweak the Framing Transposer parameters until you've got a feel you like.
            
            // var position = target.position + (Vector3.up * 5);
            // transform.position = position - (transform.forward * _radius);
            // transform.RotateAround(position, Vector3.up, Input.GetAxis("Mouse X"));
            // transform.RotateAround(position, transform.right, Input.GetAxis("Mouse Y"));
        }
    }
}