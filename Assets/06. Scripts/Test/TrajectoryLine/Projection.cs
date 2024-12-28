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
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D)); // �������� ���� ���� ����
        _physicsScene = _simulationScene.GetPhysicsScene();                                                               // �ش� ���� ���� �ùķ��̼� ������ ��Ÿ���� PhysicsScene�� ������

        foreach (Transform obj in _obstaclesParent) // �ε��� ������Ʈ�� ��� ���� ��ü�� �����ؼ� �ùķ��̼� ���� �ѱ�
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            
            if (!ghostObj.isStatic) // static�� �ƴϿ���
                _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    void Update() 
    {
        foreach (var item in _spawnedObjects)  // ��ȯ�� ������Ʈ ��ȸ
        {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    // �˵� �ùķ��̼�
    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity) 
    {
        // ���� �� ����
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene); // ���� ���� �ùķ��̼� ������ �̵���Ŵ

        ghostObj.Init(velocity, true); // ���� �� �ʱ�ȭ

        _line.positionCount = _maxPhysicsFrameIterations;   // �˵� �ִ� ���������� ����

        // �˵� �ùķ��̼� �ǽ�
        for (var i = 0; i < _maxPhysicsFrameIterations; i++) 
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostObj.transform.position);
        }

        // �˵� �ùķ��̼ǿ� ����� ���� �� ����
        Destroy(ghostObj.gameObject);
    }
}