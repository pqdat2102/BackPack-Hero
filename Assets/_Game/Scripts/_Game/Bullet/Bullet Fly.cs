using UnityEngine;

public class BulletFly : BulletAbstract
{
    public float speed = 10f; // Tốc độ di chuyển của đạn
    public float rotationSpeed = 10f; // Tốc độ xoay của đạn (để tạo cảm giác dẫn đường)
    private Transform target; // Mục tiêu (enemy)

    // Phương thức để gán mục tiêu từ bên ngoài
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    protected virtual void Update()
    {
        // Kiểm tra xem GameObject có còn active không
        if (!gameObject.activeInHierarchy) return;

        // Nếu không có mục tiêu, di chuyển thẳng theo hướng hiện tại
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            transform.parent.position += transform.parent.right * speed * Time.deltaTime;
            return;
        }

        // Tính hướng đến mục tiêu
        Vector3 directionToTarget = (target.position - transform.parent.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Xoay đạn về phía mục tiêu (tạo cảm giác dẫn đường)
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Di chuyển thẳng đến mục tiêu
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, target.position, speed * Time.deltaTime);

        // (Tùy chọn) Nếu viên đạn đến gần mục tiêu, có thể xử lý va chạm hoặc tắt viên đạn
        if (Vector3.Distance(transform.parent.position, target.position) < 0.1f)
        {
            // Xử lý va chạm hoặc tắt viên đạn (tùy vào logic game của bạn)
            // Ví dụ: gameObject.SetActive(false);
        }
    }
}