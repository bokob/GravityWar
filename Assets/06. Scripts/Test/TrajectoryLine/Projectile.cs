using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody _rb;
    Collider _collider;
    GravityObject _gravityObject;

    public Vector3 direction;           // ����
    public Vector3 initialPosition;     // �ʱ� ��ġ
    public float initialSpeed;          // �ʱ� �ӵ�
    public float mass;                  // ����

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _gravityObject = GetComponent<GravityObject>();
    }

    // �ʱ�ȭ
    public void Init(Vector3 direction, Vector3 initialPosition, float initialSpeed)
    {
        this.direction = direction;
        this.initialPosition = initialPosition;
        this.initialSpeed = initialSpeed;
        mass = _rb.mass;

        transform.position = initialPosition;
    }

    public void SetGravityAreaList(List<GravityArea> gravityAreaList)
    {
        _gravityObject._gravityAreaList = gravityAreaList;
    }
}