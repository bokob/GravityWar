using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _cameraMode;
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] GameObject _target;                // ī�޶�� �� ���
    [SerializeField] GameObject _localPlayer;           // ���� �÷��̾�

    [Header("ī�޶� �̵�")]
    [SerializeField] float _distanceFromPlayer = 3.0f;  // �÷��̾�� ������ ���� �Ÿ�
    [SerializeField] float _observerModeSpeed = 0.1f;

    [Header("ī�޶� ȸ��")]
    [SerializeField] Vector3 _currentRotation;          // ���� ȸ��
    [SerializeField] Vector3 _observerRotation;         // ���� ȸ��
    [SerializeField] float _topClamp = 70.0f;           // ī�޶� ��� ���Ѱ�
    [SerializeField] float _bottomClamp = -30.0f;       // ī�޶� �ϴ� ���Ѱ�
    [SerializeField] float _rotateSpeed = 10f;          // ȸ�� �ӵ�
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

    #region �⺻ ���
    // �⺻ ī�޶� �̵�
    void DefaultMovement()
    {
        // ��ǥ�κ��� ���� �Ÿ� ������ �ֱ�
        transform.position = _target.transform.position - transform.forward * _distanceFromPlayer;
    }

    // �⺻ ī�޶� ȸ��
    void DefaultRotation()
    {
        transform.RotateAround(_target.transform.position, _target.transform.up, _playerInput.moveMouseInput.x * _rotateSpeed); // ���� ȸ��
        transform.RotateAround(_target.transform.position, _target.transform.right, _playerInput.moveMouseInput.y * _rotateSpeed); // ���� ȸ��
        transform.LookAt(_target.transform.position);
    }
    #endregion

    #region ������ ���

    // ������ ��� �̵�
    void ObserverMovement()
    {
        Vector3 dir = Vector3.zero;

        // ���콺 ��ũ�ѽ� ����/�ƿ�
        if (_playerInput.mouseScroll > 0.0f) // ����
            dir += transform.forward;
        else if (_playerInput.mouseScroll < 0.0f) // �ܾƿ�
            dir += -transform.forward;

        // ���콺 �� ������ ���� ��, �ش� ��ġ �������� ���� �̵�
        if (_playerInput.isClickMouseWheel)
            dir += transform.up * -_playerInput.moveMouseInput.y + transform.right * -_playerInput.moveMouseInput.x;

        // ȭ��ǥ �Է� �� �̵�
        if (_playerInput.moveArrowInput != Vector3.zero)
            dir += transform.forward * _playerInput.moveArrowInput.z + transform.right * _playerInput.moveArrowInput.x;

        transform.position += dir.normalized * _observerModeSpeed;
    }

    // ������ ��� ȸ��
    void ObserverRotation()
    {
        // ��Ŭ�� �ϴ� ���� ȸ��
        if (Input.GetMouseButton(1))
        {
            _observerRotation.y += -_playerInput.moveMouseInput.y;
            _observerRotation.x += _playerInput.moveMouseInput.x;
            transform.rotation = Quaternion.Euler(_observerRotation.y, _observerRotation.x, 0);
        }
    }
    #endregion

    // ���� ����
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
