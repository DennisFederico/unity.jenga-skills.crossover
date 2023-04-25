using scriptable;
using UnityEngine;

namespace behaviours.config {
    public class SkillDataLoadConfigHolder : MonoBehaviour {
        [SerializeField] private SkillDataLoadConfigSO skillDataLoadConfig;
        public SkillDataLoadConfigSO SkillDataLoadConfig => skillDataLoadConfig;
    }
}