using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    Animator _anim;
    Collider _collider;

    Vector3 moveInput;                                    // ����Ű �Է°�
    [SerializeField] float _moveSpeed = 5f;               // �̵� �ӵ�
    [SerializeField] float _rotationSpeed = 2500f;        // ȸ�� �ӵ�

    Camera _mainCam;

    [SerializeField] bool _isGround;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _jumpForce = 5f;
    ApplyGravity _applyGravity;


    void Start()
    {
        _mainCam = Camera.main.GetComponent<Camera>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _applyGravity = GetComponentInChildren<ApplyGravity>();
    }

    void Update()
    {
        CheckGround();

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        Jump();
    }

    void FixedUpdate()
    {
        Move();
        _applyGravity.StandOnGround();
    }

    void Move()
    {
        // ĳ���� ȸ���� ǥ�� (�����)
        Debug.DrawRay(transform.position, -_applyGravity.GravityDirection * 5f, Color.blue);

        _anim.SetBool("IsMove", moveInput != Vector3.zero);
        if (moveInput == Vector3.zero) return;

        // �̵� ����
        Vector3 moveDirection = (transform.forward * moveInput.z);

        // �̵�
        _rb.MovePosition(_rb.position + moveDirection * _moveSpeed * Time.fixedDeltaTime);

        Quaternion rightDirection = Quaternion.Euler(0f, moveInput.x * (_rotationSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, _rb.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
        _rb.MoveRotation(newRotation);
    }

    void CheckGround() // ���� üũ
    {
        _isGround = Physics.CheckSphere(transform.position, 0.3f, _groundMask);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _rb.AddForce(-_applyGravity.GravityDirection * _jumpForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmos()
    {
        // ���� ��Ҵ��� ����
        Gizmos.color = (_isGround) ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f); 
    }
}
