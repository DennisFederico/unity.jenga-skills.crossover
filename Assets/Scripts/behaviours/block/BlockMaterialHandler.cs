using UnityEngine;

namespace behaviours.block {
    public class BlockMaterialHandler : MonoBehaviour {
        [SerializeField] private MeshRenderer meshRenderer;

        public void SetMaterial(Material material) {
            meshRenderer.material = material;
        }
    }
}