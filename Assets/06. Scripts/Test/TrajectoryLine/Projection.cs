using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour 
{
    [SerializeField] LineRenderer _line;
    [SerializeField] int _maxPhysicsFrameIterations = 100;
    [SerializeField] Transform _obstaclesParent;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    void Start() 
    {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene() 
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D)); // 독립적인 물리 공간 생성
        _physicsScene = _simulationScene.GetPhysicsScene();                                                               // 해당 씬의 물리 시뮬레이션 공간을 나타내는 PhysicsScene을 가져옴

        foreach (Transform obj in _obstaclesParent) // 부딪힐 오브젝트를 모두 유령 객체로 복사해서 시뮬레이션 씬에 넘김
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            
            if (!ghostObj.isStatic) // static이 아니여야
                _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    void Update() 
    {
        foreach (var item in _spawnedObjects)  // 소환된 오브젝트 순회
        {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    // 궤도 시뮬레이션
    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity) 
    {
        // 유령 공 생성
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene); // 유령 공을 시뮬레이션 씬으로 이동시킴

        ghostObj.Init(velocity, true); // 유령 공 초기화

        _line.positionCount = _maxPhysicsFrameIterations;   // 궤도 최대 프레임으로 설정

        // 궤도 시뮬레이션 실시
        for (var i = 0; i < _maxPhysicsFrameIterations; i++) 
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostObj.transform.position);
        }

        // 궤도 시뮬레이션에 사용한 유령 공 삭제
        Destroy(ghostObj.gameObject);
    }
}