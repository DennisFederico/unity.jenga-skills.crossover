using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using behaviours.config;
using model;
using scriptable;
using UnityEngine;
using UnityEngine.Networking;

namespace managers {
    [RequireComponent(typeof(SkillDataLoadConfigHolder))]
    public class StacksDataLoader : MonoBehaviour {
        public static StacksDataLoader Instance { get; private set; }
        private SkillDataLoadConfigSO _skillDataLoadConfigSO;

        private SkillDataArray AllSkillsData { get; set; }

        //I tried using a sorted list for the skill, but the combination (domain, cluster, standardid) is not unique by grade
        //Thus added the id to the skill key to make it unique
        public SortedDictionary<string, SortedList<SkillData.SkillDataKey, SkillData>> SortedSkillsByGrade { get; private set; }
        public event Action<string> OnAnyError;
        private event Action<string> OnJsonLoaded;
        private event Action OnStacksLoaded;
        
        #region StandardUnity
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
            _skillDataLoadConfigSO = GetComponent<SkillDataLoadConfigHolder>().SkillDataLoadConfig;
        }

        private void Start() {
            OnStacksLoaded += () => SceneLoadManager.Instance.LoadScene(SceneLoadManager.Scene.GameScene);
            OnJsonLoaded += ParseJson;

            StartCoroutine(_skillDataLoadConfigSO.dataLoadSource == DataLoadSource.Remote ? 
                LoadJsonFromUrl(OnJsonLoaded, OnAnyError) : 
                LoadJsonFromLocal(OnJsonLoaded, OnAnyError));
        }
        #endregion

        #region LoadAndParseJson
        private IEnumerator LoadJsonFromUrl(Action<string> onSuccessCallback, Action<string> onErrorCallback = null) {
            using UnityWebRequest request = UnityWebRequest.Get(_skillDataLoadConfigSO.baseUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                onSuccessCallback(request.downloadHandler.text);
                yield return null;
            } else {
                Debug.LogError(request.error);
                onErrorCallback?.Invoke("Error Loading Data from URL");
            }
        }

        private IEnumerator LoadJsonFromLocal(Action<string> onSuccessCallback, Action<string> onErrorCallback = null) {
            var localFilePath = Application.streamingAssetsPath + _skillDataLoadConfigSO.baseUrl;
            if (File.Exists(localFilePath)) {
                try {
                    onSuccessCallback(File.ReadAllText(localFilePath));
                } catch (Exception e) {
                    Debug.LogError(e);
                    onErrorCallback?.Invoke($"Error reading JSON file at path: {localFilePath}");
                    yield break;
                }
            } else {
                var e = $"JSON file not found at path: {localFilePath}";
                Debug.LogError(e);
                onErrorCallback?.Invoke(e);
            }
        }
        
        private void ParseJson(string json) {
            try {
                AllSkillsData = JsonUtility.FromJson<SkillDataArray>($"{{\"skills\":{json}}}");
                SortedSkillsByGrade = SplitSkillsByGrade(AllSkillsData.skills);
                
                //clearing the data from memory
                AllSkillsData.skills = null;
                AllSkillsData = null;
                
                OnStacksLoaded?.Invoke();
            } catch (Exception e) {
                Debug.LogError(e);
                OnAnyError?.Invoke("Error parsing JSON");
            }
        }
        #endregion
        
        #region SliceAndDiceStacks
        private static SortedDictionary<string, SortedList<SkillData.SkillDataKey, SkillData>> SplitSkillsByGrade(SkillData[] skills) {
            Debug.Log("Sorting ${skills.Length} skills by grade");
            var dictionary = new SortedDictionary<string, SortedList<SkillData.SkillDataKey, SkillData>>();
            foreach (var skillData in skills) {
                if (dictionary.TryGetValue(skillData.grade, out var list)) {
                    list.Add(skillData.GetSkillOrderKey(), skillData);
                } else {
                    var newList = new SortedList<SkillData.SkillDataKey, SkillData> { { skillData.GetSkillOrderKey(), skillData } };
                    dictionary.Add(skillData.grade, newList);
                }
            }
            Debug.Log($"Sorting complete - {dictionary.Count} grades found");
            if (Debug.unityLogger.logEnabled) {
                DebugParsedSkillsByGrade(dictionary);
            }
            return dictionary;
        }
        #endregion
        
        private static void DebugParsedSkillsByGrade(SortedDictionary<string, SortedList<SkillData.SkillDataKey, SkillData>> sortedSkillsByGrade) {
            int totalSkills = 0;
            foreach (var grade in sortedSkillsByGrade.Keys) {
                int gradeSkills = sortedSkillsByGrade[grade].Count;
                Debug.Log($"Grade: {grade} has {gradeSkills} skills");
                totalSkills += gradeSkills;
            }
            Debug.Log($"Total skills: {totalSkills}");
        }
    }
}