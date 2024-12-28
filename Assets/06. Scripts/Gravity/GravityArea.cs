using UnityEngine;

[RequireComponent(typeof(Collider))] // Collider ������Ʈ ������ �ڵ����� �߰�
public abstract class GravityArea : MonoBehaviour
{
    void Start()
    {
        // ���� �ȿ� ������ �� �����ϱ� ����
        transform.GetComponent<Collider>().isTrigger = true;
    }

    public abstract Vector3 GetGravityDirection(GravityObject applyGravity);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GravityObject gravityObject))
        {
            gravityObject.AddGravityArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GravityObject gravityObject))
        {
            gravityObject.RemoveGravityArea(this);
        }
    }
}
