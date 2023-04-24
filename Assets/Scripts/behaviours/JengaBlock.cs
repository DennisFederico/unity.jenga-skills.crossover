using model;
using UnityEngine;

namespace behaviours {
    public class JengaBlock : MonoBehaviour {
        [SerializeField] private MeshRenderer meshRenderer;
        
        private SkillData _skillData;
        
        public void SetSkillData(SkillData skillData) {
            _skillData = skillData;
        }
        
        public void SetMaterial(Material material) {
            meshRenderer.material = material;
        }
        
        
        // private void OnMouseOver() {
        //     if (Input.GetMouseButtonDown(1)) {
        //         GameManager.Instance.ShowInfoOnUI(transform.parent, _skillData);
        //     }
        // }
    }
}