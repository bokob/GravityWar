using UnityEngine;

/// <summary>
/// ��ü�� ������ �������� ���� ����
/// </summary>
public class GravityAreaPoint : GravityArea
{
    [SerializeField] private Vector3 _center;

    public override Vector3 GetGravityDirection(GravityObject gravityObject)
    {
        // gravityObject -> _center
        return (_center - gravityObject.transform.position).normalized;
    }
}
