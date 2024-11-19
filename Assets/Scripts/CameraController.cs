using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _target;                // 카메라로 볼 대상

    [Header("카메라 이동")]
    [SerializeField] float _distanceFromTarget = 3.0f;  // 목표와 떨어져 있을 거리

    [Header("카메라 회전")]
    [SerializeField] Vector3 _currentRotation;          // 현재 회전
    [SerializeField] float _topClamp = 70.0f;           // 카메라 상단 제한각
    [SerializeField] float _bottomClamp = -30.0f;       // 카메라 하단 제한각
    [SerializeField] float _rotateSpeed = 10f;          // 회전 속도

    void LateUpdate()
    { 
        CameraRotation();
        CameraMovement();
    }

    // 카메라 이동
    void CameraMovement()
    {
        // 목표로부터 일정 거리 떨어져 있기
        transform.position = _target.transform.position - transform.forward * _distanceFromTarget;
    }

    // 카메라 회전
    void CameraRotation()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X"); // 마우스 좌우
        float mouseY = Input.GetAxis("Mouse Y"); // 마우스 위아래

        // 입력에 의해 카메라의 현재 각도 변경
        _currentRotation.x += mouseX * _rotateSpeed;
        _currentRotation.y += mouseY * _rotateSpeed;

        // 위아래 각도 제한
        _currentRotation.y = ClampAngle(_currentRotation.y, _bottomClamp, _topClamp);

        // 새로 계산된 각도로 회전
        transform.rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);
    }

    // 각도 제한
    static public float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
        //return lfAngle;
    }
}
