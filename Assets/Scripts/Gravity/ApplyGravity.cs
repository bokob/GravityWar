using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �߷� ���� ��ü�� ����� ��ũ��Ʈ 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ApplyGravity : MonoBehaviour
{
    Rigidbody _rb;

    List<GravityArea> _gravityAreaList;
    public float _gravity_force = 1000f;

    // �߷� ����
    public Vector3 GravityDirection
    {
        get
        {
            // ������ �޴� �߷� ������ ���� ���
            if (_gravityAreaList.Count == 0) return Vector3.zero;

            // ���� �켱������ ���� �߷� ���� ��ȯ
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
        // �߷��� �޴� �������� ���� ����
        _rb.AddForce(GravityDirection * (_gravity_force * Time.fixedDeltaTime), ForceMode.Acceleration);

        // ��ü�� �� ���Ͱ� �߷� ������ �ݴ�� �� �� �ְԲ� ���� ��� (��ü�� ����� ������ �� ����)
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);

        // ���� �������� ���� ���� ������ ��ȯ
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, upRotation * _rb.rotation, Time.fixedDeltaTime * 3f);

        // ��ü ȸ��
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
