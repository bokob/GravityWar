using UnityEngine;

/// <summary>
/// ��ü�� �߽����� ������� ����
/// </summary>
public class GravityAreaCenter : GravityArea
{
    public override Vector3 GetGravityDirection(GravityObject gravityObject)
    {
        // gravityObject -> transform.point
        return (transform.position - gravityObject.transform.position).normalized;
    }
}
