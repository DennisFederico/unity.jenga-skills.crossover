using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using config;
using model;
using scriptable;
using UnityEngine;
using UnityEngine.Networking;

namespace managers {
    [RequireComponent(typeof(GameConfigHolder))]
    public class StacksManager : MonoBehaviour {
        public static StacksManager Instance { get; private set; }
        private GameConfigSO _gameConfigSO;

        //[SerializeField] private bool loadFromUrl;

        private SkillDataArray AllSkillsData { get; set; }

        //I tried using a sorted list for the skill, but the combination (domain, cluster, standardid) is not unique by grade
        //Thus added the id to the skill key to make it unique
        public SortedDictionary<string, SortedList<SkillData.SkillDataKey, SkillData>> SortedSkillsByGrade { get; private set; }
        public event Action<string> OnAnyError;
        private event Action<string> OnJsonLoaded;
        private event Action OnStacksLoaded;

        // private const string ApiUrl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        // private readonly string _localFile = Application.streamingAssetsPath + "/test-apiCallResponse.json";

        #region StandardUnity
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
            _gameConfigSO = GetComponent<GameConfigHolder>().GameConfig;
        }

        private void Start() {
            OnStacksLoaded += () => SceneLoadManager.Instance.LoadScene(SceneLoadManager.Scene.GameScene);
            OnJsonLoaded += ParseJson;

            StartCoroutine(_gameConfigSO.dataLoadSource == DataLoadSource.Remote ? 
                LoadJsonFromUrl(OnJsonLoaded, OnAnyError) : 
                LoadJsonFromLocal(OnJsonLoaded, OnAnyError));
        }
        #endregion

        #region LoadAndParseJson
        private IEnumerator LoadJsonFromUrl(Action<string> onSuccessCallback, Action<string> onErrorCallback = null) {
            using UnityWebRequest request = UnityWebRequest.Get(_gameConfigSO.baseUrl);
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
            var localFilePath = Application.streamingAssetsPath + _gameConfigSO.baseUrl;
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

        #region PublicMethods

        public int GetMaxStacks() {
            return _gameConfigSO.maxStacks;
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