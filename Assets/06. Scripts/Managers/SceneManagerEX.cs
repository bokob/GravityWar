using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour
{
    static SceneManagerEX _instance;
    public static SceneManagerEX Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성
            if (_instance == null)
            {
                // Hierarchy에 없을 때 생성
                GameObject go = new GameObject("SceneManagerEX");
                _instance = go.AddComponent<SceneManagerEX>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    void Awake()
    {
        // 이미 인스턴스가 있으면 중복 제거
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    List<string> GetSceneNameList()
    {
        List<string> sceneNameList = new List<string>();
        int buildedSceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < buildedSceneCount; i++)
        {
            // 빌드 설정에 등록된 씬의 경로 가져오기
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNameList.Add(sceneName);
        }
        return sceneNameList;
    }

    void OnEnable()
    {
        // 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        Debug.Log("씬 이벤트 해제");

        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // 씬 로드 시 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("새로운 씬이 로드됨: " + scene.name);
    }

    // 씬 언로드 시 호출
    void OnSceneUnloaded(Scene scene)
    {

    }

    // 씬 전환
    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // 현재 씬 이름 반환
    public string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
