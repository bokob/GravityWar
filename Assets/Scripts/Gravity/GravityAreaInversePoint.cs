using UnityEngine;

/// <summary>
/// ��ü�� ������ �������� �о�� ����
/// </summary>
public class GravityAreaInversePoint : GravityArea
{
    [SerializeField] private Vector3 _center;

    public override Vector3 GetGravityDirection(ApplyGravity applyGravity)
    {
        // _center -> applyGravity
        return (applyGravity.transform.position - _center).normalized;
    }
}
