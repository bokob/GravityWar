using UnityEngine;

public class Cannon : MonoBehaviour {
    [SerializeField] Projection _projection;

    void Update() 
    {
        HandleControls();

        // �˵� �ùķ��̼�
        _projection.SimulateTrajectory(_ballPrefab, _ballSpawn.position, _ballSpawn.forward * _force);
    }

    #region Handle Controls

    [SerializeField] Ball _ballPrefab;
    [SerializeField] float _force = 20;
    [SerializeField] Transform _ballSpawn;
    [SerializeField] Transform _barrelPivot;
    [SerializeField] float _rotateSpeed = 30;

    // ���� ����
    void HandleControls() 
    {
        if (Input.GetKey(KeyCode.S)) _barrelPivot.Rotate(Vector3.right * _rotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.W)) _barrelPivot.Rotate(Vector3.left * _rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.down * _rotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            // ���� ����
            var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
            spawned.Init(_ballSpawn.forward * _force, false);
        }
    }

    #endregion
}