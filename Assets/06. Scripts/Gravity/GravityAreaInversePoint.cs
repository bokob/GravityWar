using UnityEngine;

/// <summary>
/// ��ü�� ������ �������� �о�� ����
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
