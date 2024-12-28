using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _target;                // ī�޶�� �� ���
    [SerializeField] GameObject _localPlayer;           // ���� �÷��̾�

    [Header("ī�޶� �̵�")]
    [SerializeField] float _distanceFromTarget = 3.0f;  // ��ǥ�� ������ ���� �Ÿ�

    [Header("ī�޶� ȸ��")]
    [SerializeField] Vector3 _currentRotation;          // ���� ȸ��
    [SerializeField] float _topClamp = 70.0f;           // ī�޶� ��� ���Ѱ�
    [SerializeField] float _bottomClamp = -30.0f;       // ī�޶� �ϴ� ���Ѱ�
    [SerializeField] float _rotateSpeed = 10f;          // ȸ�� �ӵ�

    [SerializeField] Transform _aimPos;

    [SerializeField] PlayerInput _playerInput;

    void Start()
    {
        //_localPlayer = 
    }

    void LateUpdate()
    { 
        CameraRotation();
        if(Input.GetMouseButton(1))
        {
            transform.position = _aimPos.position;
            Debug.Log("��Ŭ��");
        }
        else
            CameraMovement();
    }

    // ī�޶� �̵�
    void CameraMovement()
    {
        // ��ǥ�κ��� ���� �Ÿ� ������ �ֱ�
        transform.position = _target.transform.position - transform.forward * _distanceFromTarget;
    }

    // ī�޶� ȸ��
    void CameraRotation()
    {
        //// �Է¿� ���� ī�޶��� ���� ���� ����
        //_currentRotation.x += PlayerInput.moveMouseInput.x * _rotateSpeed;
        //_currentRotation.y += PlayerInput.moveMouseInput.y * _rotateSpeed;

        // ���Ʒ� ���� ����
        _currentRotation.y = ClampAngle(_currentRotation.y, _bottomClamp, _topClamp);

        // ���� ���� ������ ȸ��
        transform.rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);
    }

    // ���� ����
    static public float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
        //return lfAngle;
    }

    // ���� �������� ī�޶� �̵�
    void SetAimPosition()
    {

    }
}
