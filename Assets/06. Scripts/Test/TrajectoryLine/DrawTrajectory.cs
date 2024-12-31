using System;
using UnityEngine;
using static Define;

[RequireComponent(typeof(LineRenderer))]
public class DrawTrajectory : MonoBehaviour
{
    #region Members
    LineRenderer trajectoryLine;
    //[SerializeField, Tooltip("The marker will show where the projectile will hit")] Transform hitMarker;
    [SerializeField, Range(10, 100), Tooltip("LineRenderer�� �����ϴ� �ִ� point�� ����")] int maxPoints = 50;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("trajectory ��꿡 ����ϴ� �ð� ����")] float increment = 0.025f;
    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    // Raycast ������, �ø��� �浹 ���� ��Ȯ���� ����
    float rayOverlap = 1.1f;

    GravityObject _gravityObject;
    [SerializeField] LayerMask _blockRaycast;
    #endregion

    void Start()
    {
        if (trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();

        SetTrajectoryVisible(true);
    }

    /// <summary>
    /// ����ü �˵� ����
    /// </summary>
    /// <param name="projectile"> ����ü </param>
    public void PredictTrajectory(Projectile projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass); // �� �̻��ؼ� ������ ����
        Vector3 position = projectile.initialPosition;  // �ʱ� ��ġ ����
        Vector3 nextPosition;                           // ���� ��ġ
        float overlap;

        UpdateLineRender(maxPoints, (0, position)); // ù ��° ���� ��ġ ����

        for (int i = 1; i < maxPoints; i++) // �˵��� �� �� ���
        {
            // �ӵ� �����ϰ�, ���� ���� ��ġ ������Ʈ
            velocity = CalculateNewVelocity(projectile, velocity, increment);
            nextPosition = position + velocity * increment;

            // ����ü�� ������ �����̸� Racast ���̸� �Ѿ���� ������ ���� �� �����Ƿ�, Raycast ���̸� ���� �÷��� �浹 ���ɼ��� ��ġ�� �ʱ� ���ؼ� ���
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            // // �浹 ���� �Ǹ� ����
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap, _blockRaycast))
            {
                Debug.Log("�浹 ����");

                UpdateLineRender(i, (i - 1, hit.point));
                //MoveHitMarker(hit);
                break;
            }

            // �浹�� ������ marker ���� ��� LineRenderer�� ������
            //hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position)); //Unneccesary to set count here, but not harmful
        }
    }
    /// <summary>
    /// Allows us to set line count and an induvidual position at the same time
    /// </summary>
    /// <param name="count">Number of points in our line</param>
    /// <param name="pointPos">The position of an induvidual point</param>
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos) // (int, Vector3) Ʃ��
    {
        trajectoryLine.positionCount = count;                     // ���� ���� ����
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos); // �� ���� ���� ����
        //Debug.Log(pointPos.point + " " + pointPos.pos);
    }

    /// <summary>
    /// �� �ӵ� ���
    /// </summary>
    /// <param name="velocity"> �ӵ� </param>
    /// <param name="drag"> ���� ���� </param>
    /// <param name="increment"> ��� �ð� ���� </param>
    /// <returns></returns>
    private Vector3 CalculateNewVelocity(Projectile projectile, Vector3 velocity, float increment)
    {
        //velocity += Physics.gravity * increment;            // GravityObject�� �������� �ٲ� ����
        Debug.Log(projectile.GetComponent<GravityObject>().GravityDirection);
        Debug.DrawRay(transform.position, projectile.GetComponent<GravityObject>().GravityDirection * 20f, Color.green);
        velocity += projectile.GetComponent<GravityObject>().GravityDirection * increment;            // GravityObject�� �������� �ٲ� ����
        //velocity *= Mathf.Clamp01(1f - drag * increment);   // ���� �����̶� ���� ���� ���� �ٲ� ����
        return velocity;
    }

    //private void MoveHitMarker(RaycastHit hit)  // ����ü Ÿ�� �����̱�
    //{
    //    hitMarker.gameObject.SetActive(true);

    //    // Offset marker from surface
    //    float offset = 0.025f;
    //    hitMarker.position = hit.point + hit.normal * offset;
    //    hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    //}

    // �˵� ����ȭ
    public void SetTrajectoryVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
        //hitMarker.gameObject.SetActive(visible);
    }
}
