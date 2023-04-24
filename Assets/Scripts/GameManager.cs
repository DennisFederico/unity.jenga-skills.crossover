using managers;
using scriptable;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameConfigSO _gameConfig;

    private void Start() {
        var allSkills = StacksManager.Instance.SortedSkillsByGrade;

        foreach (var grade in allSkills.Keys) {
            foreach (var skillData in allSkills[grade]) {
                Debug.Log($"{skillData.Key} / {skillData.Value.ToString()}");                
            }
        }
    }
}