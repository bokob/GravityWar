using UnityEngine;

/// <summary>
/// 물체를 지정한 지점에서 밀어내는 방향
/// </summary>
public class GravityAreaInversePoint : GravityArea
{
    [SerializeField] private Vector3 _center;

    public override Vector3 GetGravityDirection(GravityObject gravityObject)
    {
        // _center -> gravityObject
        return (gravityObject.transform.position - _center).normalized;
    }
}
