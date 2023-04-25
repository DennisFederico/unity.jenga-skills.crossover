using UnityEngine;

namespace behaviours.block {
    [RequireComponent(typeof(Outline))]
    public class BlockOutlineHandler : MonoBehaviour {
        private Outline _outline;

        private void Start() {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

        private void OnMouseEnter() {
            _outline.enabled = true;
        }

        private void OnMouseExit() {
            _outline.enabled = false;
        }
    }
}