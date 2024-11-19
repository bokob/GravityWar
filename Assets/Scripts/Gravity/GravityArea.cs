using UnityEngine;

[RequireComponent(typeof(Collider))] // Collider ������Ʈ ������ �ڵ����� �߰�
public abstract class GravityArea : MonoBehaviour
{
    int _priority;

    [field : SerializeField] 
    public int Priority { get => _priority; set => _priority = value; }

    void Start()
    {
        // ���� �ȿ� ������ �� �����ϱ� ����
        transform.GetComponent<Collider>().isTrigger = true;
    }

    public abstract Vector3 GetGravityDirection(ApplyGravity applyGravity);

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ApplyGravity applyGravity))
        {
            applyGravity.AddGravityArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ApplyGravity applyGravity))
        {
            applyGravity.RemoveGravityArea(this);
        }
    }
}
