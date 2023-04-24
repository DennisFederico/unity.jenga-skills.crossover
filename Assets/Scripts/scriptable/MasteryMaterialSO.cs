using System;
using model;
using UnityEngine;

namespace scriptable {
    /// <summary>
    /// Scriptable object that holds the material for each mastery level.
    /// </summary>
    [CreateAssetMenu(menuName = "MasteryMaterialSO")]
    public class MasteryMaterialSO : ScriptableObject {
        [SerializeField] private Material[] materials = new Material[Enum.GetNames(typeof(SkillMasteryLevel)).Length];
        
        public Material GetMaterial(SkillMasteryLevel masteryLevel) {
            return materials[(int) masteryLevel];
        }
        
        public Material GetMaterial(int masteryLevel) {
            return materials[masteryLevel];
        }
    }
}