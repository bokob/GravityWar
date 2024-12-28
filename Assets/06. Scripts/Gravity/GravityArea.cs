using UnityEngine;

[RequireComponent(typeof(Collider))] // Collider 컴포넌트 없으면 자동으로 추가
public abstract class GravityArea : MonoBehaviour
{
    void Start()
    {
        // 영역 안에 들어왔을 때 감지하기 위함
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
