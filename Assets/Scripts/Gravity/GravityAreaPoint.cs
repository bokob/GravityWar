using UnityEngine;

/// <summary>
/// ��ü�� ������ �������� ���� ����
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
