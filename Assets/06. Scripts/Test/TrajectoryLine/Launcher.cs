using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Launcher : MonoBehaviour
{
    GravityObject _gravityObject;

    [SerializeField] GameObject _projectilePrefab;       // ����ü prefab
    [SerializeField, Range(0.0f, 50.0f)] float force;    // �߻� ��
    [SerializeField] Transform StartPosition;            // ���� ��ġ


    Vector3 shootForce;



    bool isClickRight;
    bool isClickLeftDown;

    void Start()
    {
        //PredictionManager.Instance.CopyAllObstacles();


        _gravityObject = GetComponent<GravityObject>();

        if (StartPosition == null)
            StartPosition = transform;
    } 

    void Update()
    {
        isClickRight = Input.GetMouseButton(1);
        isClickLeftDown = Input.GetMouseButtonDown(0);
    }

    private void FixedUpdate()
    {
        Aim();
    }

    void Predict()
    {
        shootForce = transform.forward;
        //PredictionManager.Instance.Predict(_projectilePrefab, StartPosition.position, shootForce, _gravityObject);
    }

    public void Aim()
    {
        if (isClickRight)
        {

            
            if (isClickLeftDown)
            {
                PredictTrajectory(StartPosition.position);
                Launch();
            }
        }
        else
        {
            
        }
    }

    void Launch()
    {
        GameObject go = Instantiate(_projectilePrefab, StartPosition.position, Quaternion.identity);
        go.GetComponent<Rigidbody>().velocity += transform.forward * 10f;
    }











    public Vector3 initialVelocity; // �ʱ� �ӵ�
    public float drag = 0.0f; // ���� ���� (0�̸� ���� ����)
    public int maxIterations = 100; // �ִ� ��� �ݺ�
    public float timeStep = 0.02f; // �ð� ���� (��)
    public LineRenderer lineRenderer; // ��θ� �׸� LineRenderer
    public GravityObject dummy;

    public void PredictTrajectory(Vector3 startPosition)
    {
        Debug.Log("����");
        // �ʱ� ���� ����
        Vector3 position = startPosition;

        dummy.transform.position = StartPosition.position;
        Vector3 velocity = transform.forward * 10f;

        lineRenderer.positionCount = maxIterations;

        for (int i = 0; i < maxIterations; i++)
        {
            // ���� ��ġ���� �߷� ���
            Vector3 totalGravity = CalculateTotalGravity(position);

            // �߷� �� ���� ����
            velocity += totalGravity * timeStep; // �߷� ����
            //velocity *= Mathf.Clamp01(1 - drag * timeStep); // ���� ���� ����

            // ���� ��ġ ���
            Vector3 nextPosition = position + velocity * timeStep;

            // LineRenderer�� ��� ǥ��
            lineRenderer.SetPosition(i, nextPosition);

            // ���� ��ġ�� �̵�
            position = nextPosition;

            // �浹 �˻� (��: �༺ ǥ�� �浹)
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, velocity.magnitude * timeStep))
            {
                lineRenderer.positionCount = i + 1;
                break; // �浹 �� ��� ����
            }
        }
        dummy.transform.position = StartPosition.position;
    }

    // ��� GravitySource�� �������� �� �߷� ���
    private Vector3 CalculateTotalGravity(Vector3 currentPosition)
    {
        Vector3 totalGravity = Vector3.zero;

        dummy.transform.position = currentPosition;

        GravityObject d = dummy.GetComponent<GravityObject>();

        totalGravity = d.GravityDirection;

        return totalGravity * d.GravityForce * Time.fixedDeltaTime;
    }
}