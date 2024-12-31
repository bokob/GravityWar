using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region �⺻ ������Ʈ
    Rigidbody _rb;
    Animator _anim;
    Collider _collider;
    #endregion

    #region �÷��̾� ������
    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _moveSpeed = 5f;               // �̵� �ӵ�
    [SerializeField] float _rotationSpeed = 2500f;        // ȸ�� �ӵ�
    [SerializeField] float _jumpForce = 5f;               // ���� ��
    [SerializeField] float _jumpTimeoutDelta;             // ���� ��Ÿ�� ����
    [SerializeField] float _jumpTimeout = 0.50f;          // ���� ��Ÿ��
    [SerializeField] float _fallTimeoutDelta;             // ���� ��Ÿ�� ����
    [SerializeField] float _fallTimeout = 1.0f;           // ���� ��Ÿ��
    GravityObject _gravityObject;
    #endregion

    Camera _mainCam;
    PlayerInput _playerInput;
    PlayerStatus _playerStatus;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();

        _playerInput = GetComponent<PlayerInput>();
        _playerStatus = GetComponent<PlayerStatus>();

        _mainCam = Camera.main.GetComponent<Camera>();

        _gravityObject = GetComponentInChildren<GravityObject>();

        _jumpTimeoutDelta = _jumpTimeout; // ���� ��Ÿ�� �ʱ�ȭ
    }

    void Update()
    {
        CheckGround();
        Jump();
    }

    void FixedUpdate()
    {
        Move();
        StandOnGround();
    }

    // �߷� ���⿡ �°� �÷��̾� ����
    public void StandOnGround()
    {
        // ��ü�� �� ���Ͱ� �߷� ������ �ݴ�� �� �� �ְԲ� ���� ��� (��ü�� ����� ������ �� ����)
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -_gravityObject.GravityDirection);

        // ���� �������� ���� ���� ������ ��ȯ
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, upRotation * _rb.rotation, Time.fixedDeltaTime * 3f);

        // ��ü ȸ��
        _rb.MoveRotation(newRotation);
    }

    void Move()
    {
        // ĳ���� ȸ���� ǥ�� (�����)
        Debug.DrawRay(transform.position, -_gravityObject.GravityDirection * 5f, Color.blue);

        if (_playerStatus.IsFall) return; // ���� ���� ���� �̵� �Ұ�

        _anim.SetBool("IsMove", _playerInput.moveKeyInput != Vector3.zero);
        if (_playerInput.moveKeyInput == Vector3.zero) return;

        // �̵� ����
        Vector3 moveDirection = (transform.forward * _playerInput.moveKeyInput.z);

        // �̵�
        _rb.MovePosition(_rb.position + moveDirection * _moveSpeed * Time.fixedDeltaTime);

        // �̵� �������� ȸ��
        Quaternion rightDirection = Quaternion.Euler(0f, _playerInput.moveKeyInput.x * (_rotationSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, _rb.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
        _rb.MoveRotation(newRotation);
    }

    void CheckGround() // ���� üũ
    {
        _playerStatus.IsGround = Physics.CheckSphere(transform.position, 0.3f, _groundMask);
        _anim.SetBool("IsGround", _playerStatus.IsGround);
    }

    void Jump()
    {
        if(_playerStatus.IsGround) // ���鿡 �ִ� ���
        {
            _playerStatus.IsFall = false;     // ���� ���� X
            _fallTimeoutDelta = _fallTimeout; // ���� �ð� �ʱ�ȭ

            // �ִϸ��̼� �ʱ�ȭ
            _anim.SetBool("IsJump", false);
            _anim.SetBool("IsFall", false);

            if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f) // ��Ÿ�� ������, 'Space' ������ ����
            {
                if (_anim.GetBool("IsJump") == false && _anim.GetBool("IsFall") == false)
                {
                    _anim.Play("JumpingUp");
                    _anim.SetBool("IsJump", true);
                    _rb.AddForce(-_gravityObject.GravityDirection * _jumpForce, ForceMode.Impulse);
                }
            }

            if (_jumpTimeoutDelta >= 0.0f) // ���� ��Ÿ�� ���
                _jumpTimeoutDelta -= Time.deltaTime;
        }
        else // ���߿� �ִ� ���
        {
            _jumpTimeoutDelta = _jumpTimeout; // ���� ��Ÿ�� �ʱ�ȭ

            if (_fallTimeoutDelta >= 0.0f)
                _fallTimeoutDelta -= Time.deltaTime;
            else
            {
                _playerStatus.IsFall = true;
                _anim.SetBool("IsFall", true);
            }
            _anim.SetBool("IsJump", false);
        }
    }

    void OnDrawGizmos()
    {
        // ���� ��Ҵ��� ���� �ð�ȭ
        if (_playerStatus != null)
        {
            Gizmos.color = (_playerStatus.IsGround) ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}