using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Launcher : MonoBehaviour
{
    GravityObject _gravityObject;

    [SerializeField] GameObject _projectilePrefab;       // 투사체 prefab
    [SerializeField, Range(0.0f, 50.0f)] float force;    // 발사 힘
    [SerializeField] Transform StartPosition;            // 시작 위치


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











    public Vector3 initialVelocity; // 초기 속도
    public float drag = 0.0f; // 공기 저항 (0이면 저항 없음)
    public int maxIterations = 100; // 최대 계산 반복
    public float timeStep = 0.02f; // 시간 간격 (초)
    public LineRenderer lineRenderer; // 경로를 그릴 LineRenderer
    public GravityObject dummy;

    public void PredictTrajectory(Vector3 startPosition)
    {
        Debug.Log("시작");
        // 초기 상태 설정
        Vector3 position = startPosition;

        dummy.transform.position = StartPosition.position;
        Vector3 velocity = transform.forward * 10f;

        lineRenderer.positionCount = maxIterations;

        for (int i = 0; i < maxIterations; i++)
        {
            // 현재 위치에서 중력 계산
            Vector3 totalGravity = CalculateTotalGravity(position);

            // 중력 및 저항 적용
            velocity += totalGravity * timeStep; // 중력 적용
            //velocity *= Mathf.Clamp01(1 - drag * timeStep); // 공기 저항 적용

            // 다음 위치 계산
            Vector3 nextPosition = position + velocity * timeStep;

            // LineRenderer로 경로 표시
            lineRenderer.SetPosition(i, nextPosition);

            // 다음 위치로 이동
            position = nextPosition;

            // 충돌 검사 (예: 행성 표면 충돌)
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, velocity.magnitude * timeStep))
            {
                lineRenderer.positionCount = i + 1;
                break; // 충돌 시 계산 종료
            }
        }
        dummy.transform.position = StartPosition.position;
    }

    // 모든 GravitySource를 기준으로 총 중력 계산
    private Vector3 CalculateTotalGravity(Vector3 currentPosition)
    {
        Vector3 totalGravity = Vector3.zero;

        dummy.transform.position = currentPosition;

        GravityObject d = dummy.GetComponent<GravityObject>();

        totalGravity = d.GravityDirection;

        return totalGravity * d.GravityForce * Time.fixedDeltaTime;
    }
}