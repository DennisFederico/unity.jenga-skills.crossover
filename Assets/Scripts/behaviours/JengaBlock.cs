using model;
using UnityEngine;

namespace behaviours {
    public class JengaBlock : MonoBehaviour {
        [SerializeField] private MeshRenderer meshRenderer;
        private SkillMasteryLevel _masteryLevel;
        private SkillData _skillData;
        
        public void SetSkillData(SkillData skillData) {
            _skillData = skillData;
        }
        
        public void SetMaterial(SkillMasteryLevel mastery, Material material) {
            _masteryLevel = mastery;
            meshRenderer.material = material;
        }
        
        public SkillMasteryLevel GetMasteryLevel() {
            return _masteryLevel;
        }
        
        // private void OnMouseOver() {
        //     if (Input.GetMouseButtonDown(1)) {
        //         GameManager.Instance.ShowInfoOnUI(transform.parent, _skillData);
        //     }
        // }
    }
}