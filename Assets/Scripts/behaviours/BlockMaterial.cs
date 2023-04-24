using UnityEngine;

namespace behaviours {
    public class BlockMaterial : MonoBehaviour {
        [SerializeField] private MeshRenderer meshRenderer;
        
        public void SetMaterial(Material material) {
            meshRenderer.material = material;
        }
    }
}