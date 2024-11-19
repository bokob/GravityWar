using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 중력 받을 물체에 적용될 스크립트 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ApplyGravity : MonoBehaviour
{
    Rigidbody _rb;

    List<GravityArea> _gravityAreaList;
    public float _gravity_force = 1000f;

    // 중력 방향
    public Vector3 GravityDirection
    {
        get
        {
            // 영향을 받는 중력 구역이 없는 경우
            if (_gravityAreaList.Count == 0) return Vector3.zero;

            // 가장 우선순위가 높은 중력 방향 반환
            _gravityAreaList.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return _gravityAreaList.Last().GetGravityDirection(this).normalized;
        }
    }

    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _gravityAreaList = new List<GravityArea>();
    }

    public void StandOnGround()
    {
        // 중력을 받는 방향으로 힘을 가함
        _rb.AddForce(GravityDirection * (_gravity_force * Time.fixedDeltaTime), ForceMode.Acceleration);

        // 물체의 위 벡터가 중력 방향의 반대로 갈 수 있게끔 각도 계산 (물체가 제대로 서있을 수 있음)
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);

        // 원래 각도에서 새로 계산된 각도로 전환
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, upRotation * _rb.rotation, Time.fixedDeltaTime * 3f);

        // 물체 회전
        _rb.MoveRotation(newRotation);
    }

    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreaList.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreaList.Remove(gravityArea);
    }
}
