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
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // Hierarchy�� ���� �� ����
                GameObject go = new GameObject("SceneManagerEX");
                _instance = go.AddComponent<SceneManagerEX>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    void Awake()
    {
        // �̹� �ν��Ͻ��� ������ �ߺ� ����
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
            // ���� ������ ��ϵ� ���� ��� ��������
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNameList.Add(sceneName);
        }
        return sceneNameList;
    }

    void OnEnable()
    {
        // �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        Debug.Log("�� �̺�Ʈ ����");

        // �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // �� �ε� �� ȣ��
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("���ο� ���� �ε��: " + scene.name);
    }

    // �� ��ε� �� ȣ��
    void OnSceneUnloaded(Scene scene)
    {

    }

    // �� ��ȯ
    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // ���� �� �̸� ��ȯ
    public string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
