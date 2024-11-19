using UnityEngine;

/// <summary>
/// ��ü�� �߽����� ������� ����
/// </summary>
public class GravityAreaCenter : GravityArea
{
    public override Vector3 GetGravityDirection(ApplyGravity applyGravity)
    {
        // applyGravity -> transform.point
        return (transform.position - applyGravity.transform.position).normalized;
    }
}
