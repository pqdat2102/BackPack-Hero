using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private IMove moveStrategy;
    private Transform target;
    private float speed;
    private BulletController bulletController;

    private void Awake()
    {
        // Lấy BulletController từ parent
        bulletController = GetComponentInParent<BulletController>();
        if (bulletController == null)
        {
            Debug.LogError("BulletController not found in parent hierarchy!");
        }
    }

    public void Initialize(BulletData bulletData, Transform target, IMove movementStrategy)
    {
        this.target = target;
        this.moveStrategy = movementStrategy;
        this.speed = bulletData.speed;
    }

    private void Update()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Sử dụng transform của parent (bullet root) để di chuyển
            moveStrategy.Move(transform.parent, speed, Time.deltaTime, target);
        }
        else
        {
            // Fallback movement
            transform.parent.position += transform.parent.right * speed * Time.deltaTime;

            // Yêu cầu parent despawn nếu cần
            bulletController?.RequestDespawn();
        }
    }
}