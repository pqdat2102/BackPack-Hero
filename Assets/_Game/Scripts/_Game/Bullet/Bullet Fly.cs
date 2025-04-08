using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : BulletAbstract
{
    public float speed = 10f; // Tốc độ di chuyển của đạn
    public float detectionRange = 10f; // Phạm vi tìm enemy
    private Transform target; // Mục tiêu (enemy)
    private float findNewTargetInterval = 0.5f; // Thời gian giữa các lần tìm mục tiêu mới
    private float findNewTargetTimer = 0f; // Bộ đếm thời gian để tìm mục tiêu mới

    protected override void Start()
    {
        // Tìm target ngay khi đạn được sinh ra
        FindTarget();
    }

    protected virtual void Update()
    {
        // Kiểm tra xem mục tiêu có còn tồn tại không
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            // Tăng bộ đếm thời gian
            findNewTargetTimer += Time.deltaTime;
            
            // Nếu đã đến thời điểm tìm mục tiêu mới
            if (findNewTargetTimer >= findNewTargetInterval)
            {
                FindTarget();
                findNewTargetTimer = 0f;
            }
            
            // Nếu không có mục tiêu, di chuyển theo hướng hiện tại
            transform.parent.position += transform.parent.right * speed * Time.deltaTime;
            return;
        }

        // Di chuyển đạn về phía enemy
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, target.position, speed * Time.deltaTime);

        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(
                target.position.y - transform.parent.position.y, target.position.x - transform.parent.position.x) * Mathf.Rad2Deg));
    }

    void FindTarget()
    {
        // Tìm tất cả GameObject có tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        // Tìm enemy gần nhất trong phạm vi detectionRange
        foreach (GameObject enemy in enemies)
        {
            // Kiểm tra xem enemy có đang active không
            if (!enemy.activeInHierarchy) continue;
            
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= detectionRange)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy; // Gán target là enemy gần nhất (nếu có)

        if (target != null)
        {
            Debug.Log("Enemy gần nhất: " + target.name, target.gameObject);
        }
        else
        {
            Debug.Log("Không tìm thấy enemy trong phạm vi!");
        }
    }
}
