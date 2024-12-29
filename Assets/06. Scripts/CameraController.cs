using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _cameraMode;
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] GameObject _target;                // 카메라로 볼 대상
    [SerializeField] GameObject _localPlayer;           // 로컬 플레이어

    [Header("카메라 이동")]
    [SerializeField] float _distanceFromPlayer = 3.0f;  // 플레이어와 떨어져 있을 거리
    [SerializeField] float _observerModeSpeed = 0.1f;

    [Header("카메라 회전")]
    [SerializeField] Vector3 _currentRotation;          // 현재 회전
    [SerializeField] Vector3 _observerRotation;         // 현재 회전
    [SerializeField] float _topClamp = 70.0f;           // 카메라 상단 제한각
    [SerializeField] float _bottomClamp = -30.0f;       // 카메라 하단 제한각
    [SerializeField] float _rotateSpeed = 10f;          // 회전 속도
    [SerializeField] float _scrollSpeed = 1.5f;

    [SerializeField] Vector3 _defaultPos;
    [SerializeField] Vector3 _o;
    [SerializeField] Vector3 _aimPos;


    void Start()
    {
        _playerInput = transform.parent.GetChild(1).GetComponent<PlayerInput>();

        //_localPlayer = 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        switch (_cameraMode)
        {
            case Define.CameraMode.Default:
                DefaultRotation();
                DefaultMovement();
                break;
            case Define.CameraMode.Aim:
                break;
            case Define.CameraMode.Observer:
                ObserverMovement();
                ObserverRotation();
                break;
            case Define.CameraMode.Tracer:
                break;
        }
    }

    public void SetMode()
    {

    }

    #region 기본 모드
    // 기본 카메라 이동
    void DefaultMovement()
    {
        // 목표로부터 일정 거리 떨어져 있기
        transform.position = _target.transform.position - transform.forward * _distanceFromPlayer;
    }

    // 기본 카메라 회전
    void DefaultRotation()
    {
        transform.RotateAround(_target.transform.position, _target.transform.up, _playerInput.moveMouseInput.x * _rotateSpeed); // 수평 회전
        transform.RotateAround(_target.transform.position, _target.transform.right, _playerInput.moveMouseInput.y * _rotateSpeed); // 수직 회전
        transform.LookAt(_target.transform.position);
    }
    #endregion

    #region 관찰자 모드

    // 관찰자 모드 이동
    void ObserverMovement()
    {
        Vector3 dir = Vector3.zero;

        // 마우스 스크롤시 줌인/아웃
        if (_playerInput.mouseScroll > 0.0f) // 줌인
            dir += transform.forward;
        else if (_playerInput.mouseScroll < 0.0f) // 줌아웃
            dir += -transform.forward;

        // 마우스 휠 누르고 있을 시, 해당 위치 기준으로 평행 이동
        if (_playerInput.isClickMouseWheel)
            dir += transform.up * -_playerInput.moveMouseInput.y + transform.right * -_playerInput.moveMouseInput.x;

        // 화살표 입력 시 이동
        if (_playerInput.moveArrowInput != Vector3.zero)
            dir += transform.forward * _playerInput.moveArrowInput.z + transform.right * _playerInput.moveArrowInput.x;

        transform.position += dir.normalized * _observerModeSpeed;
    }

    // 관찰자 모드 회전
    void ObserverRotation()
    {
        // 우클릭 하는 동안 회전
        if (Input.GetMouseButton(1))
        {
            _observerRotation.y += -_playerInput.moveMouseInput.y;
            _observerRotation.x += _playerInput.moveMouseInput.x;
            transform.rotation = Quaternion.Euler(_observerRotation.y, _observerRotation.x, 0);
        }
    }
    #endregion

    // 각도 제한
    static public float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
        //return lfAngle;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
    }
}
