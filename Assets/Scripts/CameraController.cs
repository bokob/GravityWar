using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _target;                // ī�޶�� �� ���

    [Header("ī�޶� �̵�")]
    [SerializeField] float _distanceFromTarget = 3.0f;  // ��ǥ�� ������ ���� �Ÿ�

    [Header("ī�޶� ȸ��")]
    [SerializeField] Vector3 _currentRotation;          // ���� ȸ��
    [SerializeField] float _topClamp = 70.0f;           // ī�޶� ��� ���Ѱ�
    [SerializeField] float _bottomClamp = -30.0f;       // ī�޶� �ϴ� ���Ѱ�
    [SerializeField] float _rotateSpeed = 10f;          // ȸ�� �ӵ�

    void LateUpdate()
    { 
        CameraRotation();
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
        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X"); // ���콺 �¿�
        float mouseY = Input.GetAxis("Mouse Y"); // ���콺 ���Ʒ�

        // �Է¿� ���� ī�޶��� ���� ���� ����
        _currentRotation.x += mouseX * _rotateSpeed;
        _currentRotation.y += mouseY * _rotateSpeed;

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
}
