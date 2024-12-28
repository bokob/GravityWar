using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [SerializeField] float _distanceFromTarget = 3.0f;  // ��ǥ�� ������ ���� �Ÿ�
    [SerializeField] Vector2 mouseMoveInput; // ���콺 ������
    [SerializeField] float speed;
    [SerializeField] Vector3 _currentRotation; // ���� ȸ�� ��


    [SerializeField] Transform _target;



    Define.CameraMode _mode;

    void Start()
    {
        
    }

    void Update()
    {
        // ���콺 ������
        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");
        mouseMoveInput = new Vector2(x, y).normalized;
    }

    void CameraMovement()
    {
        // ��ǥ�κ��� ���� �Ÿ� ������ �ֱ�
        transform.position = _target.transform.position - transform.forward * _distanceFromTarget;
    }

    void LateUpdate()
    {
        DefaultVioew();
        CameraMovement();
    }

    public void DefaultVioew()
    {
        _currentRotation.x += mouseMoveInput.x;
        _currentRotation.y += mouseMoveInput.y;

        // Quaternion.Euler(): Ư���� Euler �����κ��� Quaternion�� �����ϴ� ���� �޼���
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);
    }

    public void Aim()
    {
        _currentRotation.x += mouseMoveInput.x;
        _currentRotation.y += mouseMoveInput.y;

        //transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        // Ư�� ���� �������� Ư�� ����(��)��ŭ ȸ���� ��Ÿ���� Quaternion
        // ĳ���� �������� �¿� ȸ��
        Quaternion horizonRotation = Quaternion.AngleAxis(_currentRotation.x, _target.up);

        // ĳ���� �������� ���Ʒ� ȸ��
        Quaternion verticalRotation = Quaternion.AngleAxis(-_currentRotation.y, _target.right);

        transform.rotation = horizonRotation * verticalRotation;
    }
}
