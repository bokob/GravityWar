using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 중력 받을 물체에 적용될 스크립트 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
    Rigidbody _rb;
    public List<GravityArea> _gravityAreaList;
    public float GravityForce = 1000f;
    public float G = 9.81f;

    PlayerStatus _playerStatus;

    // 중력 방향
    public Vector3 GravityDirection
    {
        get { return LawOfUniversalGravity(); }
    }

    Vector3 LawOfUniversalGravity()
    {
        if (_gravityAreaList == null || _gravityAreaList.Count == 0) return Vector3.zero;

        // 중력의 총합이 0이 되어 표류하지 않도록 하기 위함
        // 다른 행성에 있는데, 중력이 큰 행성에 끌려가지 않기 위함
        // 이를 위한 가장 가까운 행성과의 거리, 힘, 방향
        float closetPlanet = 9999999f;
        float closetPlanetForce = 0.0f;
        Vector3 closetPlanetVector = Vector3.zero;

        float objectMass = _rb.mass;
        Vector3 totalForce = Vector3.zero;
        for (int i = 0; i < _gravityAreaList.Count; i++)
        {
            float pivotMass = _gravityAreaList[i].GetComponent<Rigidbody>().mass;                  // 행성 질량
            Vector3 distVector = _gravityAreaList[i].transform.position - transform.position;      // 행성과의 거리 벡터
            float dist = distVector.magnitude;                                                     // 행성과의 거리
            float force = G * pivotMass * objectMass / Mathf.Pow(dist, 2);                         // 만유인력

            // 플레이어 전용
            if(closetPlanet > dist)
            {
                closetPlanet = dist;
                closetPlanetForce = force;
                closetPlanetVector = distVector;
            }

            totalForce += distVector.normalized * force;                                           // 타겟에 적용되는 모든 힘 더하기
        }

        // 플레이어의 경우
        if (gameObject.TryGetComponent(out _playerStatus))
        {
            if (_playerStatus.IsFall)
            {
                totalForce = closetPlanetVector.normalized * closetPlanetForce;
            }
        }

        return totalForce.normalized;
    }

    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _gravityAreaList = new List<GravityArea>();

        _playerStatus = GetComponent<PlayerStatus>();
    }

    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreaList.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreaList.Remove(gravityArea);
    }

    void OnDrawGizmos()
    {
        if (GravityDirection == Vector3.zero)
            return;

        Debug.DrawRay(transform.position, GravityDirection * 5f, Color.red);
    }
}