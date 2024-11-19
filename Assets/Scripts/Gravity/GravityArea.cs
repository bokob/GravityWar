using UnityEngine;

[RequireComponent(typeof(Collider))] // Collider 컴포넌트 없으면 자동으로 추가
public abstract class GravityArea : MonoBehaviour
{
    int _priority;

    [field : SerializeField] 
    public int Priority { get => _priority; set => _priority = value; }

    void Start()
    {
        // 영역 안에 들어왔을 때 감지하기 위함
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
