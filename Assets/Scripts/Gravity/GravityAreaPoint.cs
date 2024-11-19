using UnityEngine;

/// <summary>
/// 물체를 지정한 지점으로 당기는 방향
/// </summary>
public class GravityAreaPoint : GravityArea
{
    [SerializeField] private Vector3 _center;

    public override Vector3 GetGravityDirection(ApplyGravity applyGravity)
    {
        // applyGravity -> _center
        return (_center - applyGravity.transform.position).normalized;
    }
}
