using System;
using UnityEngine;
using static Define;

[RequireComponent(typeof(LineRenderer))]
public class DrawTrajectory : MonoBehaviour
{
    #region Members
    LineRenderer trajectoryLine;
    //[SerializeField, Tooltip("The marker will show where the projectile will hit")] Transform hitMarker;
    [SerializeField, Range(10, 100), Tooltip("LineRenderer를 구성하는 최대 point의 개수")] int maxPoints = 50;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("trajectory 계산에 사용하는 시간 간격")] float increment = 0.025f;
    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    // Raycast 보정용, 늘리면 충돌 감지 정확성이 증가
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
    /// 투사체 궤도 예측
    /// </summary>
    /// <param name="projectile"> 투사체 </param>
    public void PredictTrajectory(Projectile projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass); // 좀 이상해서 수정할 예정
        Vector3 position = projectile.initialPosition;  // 초기 위치 지정
        Vector3 nextPosition;                           // 다음 위치
        float overlap;

        UpdateLineRender(maxPoints, (0, position)); // 첫 번째 정점 위치 설정

        for (int i = 1; i < maxPoints; i++) // 궤도의 각 점 계산
        {
            // 속도 추정하고, 다음 예측 위치 업데이트
            velocity = CalculateNewVelocity(projectile, velocity, increment);
            nextPosition = position + velocity * increment;

            // 투사체가 빠르게 움직이면 Racast 길이를 넘어버려 추적을 못할 수 있으므로, Raycast 길이를 조금 늘려서 충돌 가능성을 놓치지 않기 위해서 사용
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            // // 충돌 감지 되면 종료
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap, _blockRaycast))
            {
                Debug.Log("충돌 감지");

                UpdateLineRender(i, (i - 1, hit.point));
                //MoveHitMarker(hit);
                break;
            }

            // 충돌이 없으면 marker 없이 계속 LineRenderer를 렌더링
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
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos) // (int, Vector3) 튜플
    {
        trajectoryLine.positionCount = count;                     // 정점 개수 지정
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos); // 각 정점 위지 지정
        //Debug.Log(pointPos.point + " " + pointPos.pos);
    }

    /// <summary>
    /// 새 속도 계산
    /// </summary>
    /// <param name="velocity"> 속도 </param>
    /// <param name="drag"> 공기 저항 </param>
    /// <param name="increment"> 계산 시간 간격 </param>
    /// <returns></returns>
    private Vector3 CalculateNewVelocity(Projectile projectile, Vector3 velocity, float increment)
    {
        //velocity += Physics.gravity * increment;            // GravityObject의 방향으로 바꿀 예정
        Debug.Log(projectile.GetComponent<GravityObject>().GravityDirection);
        Debug.DrawRay(transform.position, projectile.GetComponent<GravityObject>().GravityDirection * 20f, Color.green);
        velocity += projectile.GetComponent<GravityObject>().GravityDirection * increment;            // GravityObject의 방향으로 바꿀 예정
        //velocity *= Mathf.Clamp01(1f - drag * increment);   // 우주 공간이라 공기 저항 없게 바꿀 예정
        return velocity;
    }

    //private void MoveHitMarker(RaycastHit hit)  // 투사체 타점 움직이기
    //{
    //    hitMarker.gameObject.SetActive(true);

    //    // Offset marker from surface
    //    float offset = 0.025f;
    //    hitMarker.position = hit.point + hit.normal * offset;
    //    hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    //}

    // 궤도 가시화
    public void SetTrajectoryVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
        //hitMarker.gameObject.SetActive(visible);
    }
}
