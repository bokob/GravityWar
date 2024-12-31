using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 기본 컴포넌트
    Rigidbody _rb;
    Animator _anim;
    Collider _collider;
    #endregion

    #region 플레이어 움직임
    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _moveSpeed = 5f;               // 이동 속도
    [SerializeField] float _rotationSpeed = 2500f;        // 회전 속도
    [SerializeField] float _jumpForce = 5f;               // 점프 힘
    [SerializeField] float _jumpTimeoutDelta;             // 점프 쿨타임 계산용
    [SerializeField] float _jumpTimeout = 0.50f;          // 점프 쿨타임
    [SerializeField] float _fallTimeoutDelta;             // 낙하 쿨타임 계산용
    [SerializeField] float _fallTimeout = 1.0f;           // 낙하 쿨타임
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

        _jumpTimeoutDelta = _jumpTimeout; // 점프 쿨타임 초기화
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

    // 중력 방향에 맞게 플레이어 서기
    public void StandOnGround()
    {
        // 물체의 위 벡터가 중력 방향의 반대로 갈 수 있게끔 각도 계산 (물체가 제대로 서있을 수 있음)
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -_gravityObject.GravityDirection);

        // 원래 각도에서 새로 계산된 각도로 전환
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, upRotation * _rb.rotation, Time.fixedDeltaTime * 3f);

        // 물체 회전
        _rb.MoveRotation(newRotation);
    }

    void Move()
    {
        // 캐릭터 회전축 표시 (디버깅)
        Debug.DrawRay(transform.position, -_gravityObject.GravityDirection * 5f, Color.blue);

        if (_playerStatus.IsFall) return; // 낙하 중일 때는 이동 불가

        _anim.SetBool("IsMove", _playerInput.moveKeyInput != Vector3.zero);
        if (_playerInput.moveKeyInput == Vector3.zero) return;

        // 이동 방향
        Vector3 moveDirection = (transform.forward * _playerInput.moveKeyInput.z);

        // 이동
        _rb.MovePosition(_rb.position + moveDirection * _moveSpeed * Time.fixedDeltaTime);

        // 이동 방향으로 회전
        Quaternion rightDirection = Quaternion.Euler(0f, _playerInput.moveKeyInput.x * (_rotationSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(_rb.rotation, _rb.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
        _rb.MoveRotation(newRotation);
    }

    void CheckGround() // 지면 체크
    {
        _playerStatus.IsGround = Physics.CheckSphere(transform.position, 0.3f, _groundMask);
        _anim.SetBool("IsGround", _playerStatus.IsGround);
    }

    void Jump()
    {
        if(_playerStatus.IsGround) // 지면에 있는 경우
        {
            _playerStatus.IsFall = false;     // 낙하 상태 X
            _fallTimeoutDelta = _fallTimeout; // 낙하 시간 초기화

            // 애니메이션 초기화
            _anim.SetBool("IsJump", false);
            _anim.SetBool("IsFall", false);

            if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f) // 쿨타임 지나고, 'Space' 누르면 점프
            {
                if (_anim.GetBool("IsJump") == false && _anim.GetBool("IsFall") == false)
                {
                    _anim.Play("JumpingUp");
                    _anim.SetBool("IsJump", true);
                    _rb.AddForce(-_gravityObject.GravityDirection * _jumpForce, ForceMode.Impulse);
                }
            }

            if (_jumpTimeoutDelta >= 0.0f) // 점프 쿨타임 계산
                _jumpTimeoutDelta -= Time.deltaTime;
        }
        else // 공중에 있는 경우
        {
            _jumpTimeoutDelta = _jumpTimeout; // 점프 쿨타임 초기화

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
        // 땅에 닿았는지 여부 시각화
        if (_playerStatus != null)
        {
            Gizmos.color = (_playerStatus.IsGround) ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}