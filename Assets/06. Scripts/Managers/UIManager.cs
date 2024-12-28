using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("Title 씬")]
    [SerializeField] Transform _titleUI;

    [Tooltip("Practice 씬")]
    [SerializeField] Transform _practiceUI;

    [Tooltip("InGame 씬")]
    [SerializeField] Transform _inGameUI;


    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성
            if (_instance == null)
            {
                // Hierarchy에 없을 때 생성
                GameObject go = new GameObject("UIManager");
                _instance = go.AddComponent<UIManager>();
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



                    Debug.LogWarning("등록되지 않은 씬");
                    break;
                }
        }
    }

    // 버튼 이벤트 등록
    public void RegisterButtonEvent(Button button, Action<string> method = null, string parameter = null)
    {
        button.onClick.AddListener(()=>method(parameter));
    }
}
