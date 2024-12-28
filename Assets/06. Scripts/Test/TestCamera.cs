using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [SerializeField] float _distanceFromTarget = 3.0f;  // 목표와 떨어져 있을 거리
    [SerializeField] Vector2 mouseMoveInput; // 마우스 움직임
    [SerializeField] float speed;
    [SerializeField] Vector3 _currentRotation; // 현재 회전 값


    [SerializeField] Transform _target;



    Define.CameraMode _mode;

    void Start()
    {
        
    }

    void Update()
    {
        // 마우스 움직임
        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");
        mouseMoveInput = new Vector2(x, y).normalized;
    }

    void CameraMovement()
    {
        // 목표로부터 일정 거리 떨어져 있기
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

        // Quaternion.Euler(): 특정한 Euler 각도로부터 Quaternion을 생성하는 정적 메서드
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);
    }

    public void Aim()
    {
        _currentRotation.x += mouseMoveInput.x;
        _currentRotation.y += mouseMoveInput.y;

        //transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        // 특정 축을 기준으로 특정 각도(도)만큼 회전을 나타내는 Quaternion
        // 캐릭터 기준으로 좌우 회전
        Quaternion horizonRotation = Quaternion.AngleAxis(_currentRotation.x, _target.up);

        // 캐릭터 기준으로 위아래 회전
        Quaternion verticalRotation = Quaternion.AngleAxis(-_currentRotation.y, _target.right);

        transform.rotation = horizonRotation * verticalRotation;
    }
}
