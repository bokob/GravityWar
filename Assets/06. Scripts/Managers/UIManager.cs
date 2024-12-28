using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("Title ��")]
    [SerializeField] Transform _titleUI;

    [Tooltip("Practice ��")]
    [SerializeField] Transform _practiceUI;

    [Tooltip("InGame ��")]
    [SerializeField] Transform _inGameUI;


    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // Hierarchy�� ���� �� ����
                GameObject go = new GameObject("UIManager");
                _instance = go.AddComponent<UIManager>();
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

    void Start()
    {
        SetSceneUI(SceneManagerEX.Instance.CurrentSceneName());
    }

    public void SetSceneUI(string sceneName)
    {
        Transform UI = GameObject.Find("UI").transform;
        switch (sceneName)
        {
            case "TitleScene":
                {
                    Transform main = UI.GetChild(0);
                    Button startBtn = main.GetChild(2).GetComponent<Button>();

                    Debug.Log(startBtn.name);
                    RegisterButtonEvent(startBtn, SceneManagerEX.Instance.SwitchScene, "InGameScene");
                    break;
                }
            case "PracticeScene":
                {
                    break;
                }
            case "InGameScene":
                {




                    break;
                }
            default:
                {



                    Debug.LogWarning("��ϵ��� ���� ��");
                    break;
                }
        }
    }

    // ��ư �̺�Ʈ ���
    public void RegisterButtonEvent(Button button, Action<string> method = null, string parameter = null)
    {
        button.onClick.AddListener(()=>method(parameter));
    }
}
