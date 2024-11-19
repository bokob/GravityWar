using UnityEngine;

/// <summary>
/// 물체를 중심으로 끌어당기는 방향
/// </summary>
public class GravityAreaCenter : GravityArea
{
    public override Vector3 GetGravityDirection(ApplyGravity applyGravity)
    {
        // applyGravity -> transform.point
        return (transform.position - applyGravity.transform.position).normalized;
    }
}
