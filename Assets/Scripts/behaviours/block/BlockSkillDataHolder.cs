using model;
using UnityEngine;

namespace behaviours.block {
    public class BlockSkillDataHolder : MonoBehaviour {
        private SkillMasteryLevel _masteryLevel;
        private SkillData _skillData;

        public void SetSkillData(SkillData skillData) {
            _skillData = skillData;
            _masteryLevel = (SkillMasteryLevel)skillData.mastery;
        }

        public SkillData GetSkillData() {
            return _skillData;
        }

        public SkillMasteryLevel GetMasteryLevel() {
            return _masteryLevel;
        }
    }
}