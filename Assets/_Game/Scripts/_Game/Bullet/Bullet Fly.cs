/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : BulletAbstract
{
    public float speed = 10f; // Tốc độ di chuyển của đạn
    public float detectionRange = 10f; // Phạm vi tìm enemy
    public float rotationSpeed = 10f; // Tốc độ xoay của đạn (để tạo cảm giác dẫn đường)
    private Transform target; // Mục tiêu (enemy)
    private EnemyDamageReceiver targetDamageReceiver; // Tham chiếu đến EnemyDamageReceiver của target
    private BulletDamageSender damageSender; // Tham chiếu đến BulletDamageSender để lấy sát thương

    protected override void Start()
    {
        // Lấy BulletDamageSender từ cùng GameObject
        damageSender = GetComponent<BulletDamageSender>();
        //if (damageSender == null)
        //{
        //    Debug.LogError("BulletDamageSender not found on " + gameObject.name, gameObject);
        //}

        // Chọn mục tiêu ngay khi viên đạn được bắn ra
        FindTarget();
    }

    protected virtual void Update()
    {
        // Kiểm tra xem mục tiêu có còn tồn tại và còn sống không
        if (target == null || !target.gameObject.activeInHierarchy || IsTargetDead())
        {
            // Nếu mục tiêu không còn, tìm mục tiêu mới ngay lập tức
            FindTarget();

            // Nếu không có mục tiêu, di chuyển thẳng theo hướng hiện tại
            if (target == null)
            {
                transform.parent.position += transform.parent.right * speed * Time.deltaTime;
                return;
            }
        }

        // Tính hướng đến mục tiêu
        Vector3 directionToTarget = (target.position - transform.parent.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Xoay đạn về phía mục tiêu (tạo cảm giác dẫn đường)
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Di chuyển thẳng đến mục tiêu
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, target.position, speed * Time.deltaTime);
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        EnemyDamageReceiver closestEnemyDamageReceiver = null;
        float bulletDamage = GetBulletDamage(); // Lấy sát thương của viên đạn

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
            if (damageReceiver == null || damageReceiver.IsDead()) continue;

            // Bỏ qua enemy nếu HP của nó nhỏ hơn hoặc bằng sát thương của viên đạn
            // (coi như enemy này sẽ chết sau phát bắn này)
            if (damageReceiver.GetCurrentHP() <= bulletDamage) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= detectionRange)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
                closestEnemyDamageReceiver = damageReceiver;
            }
        }

        target = closestEnemy;
        targetDamageReceiver = closestEnemyDamageReceiver;

        *//*if (target != null)
        {
            Debug.Log("Enemy gần nhất: " + target.name, target.gameObject);
        }
        else
        {
            Debug.Log("Không tìm thấy enemy trong phạm vi!");
        }*//*
    }

    // Lấy sát thương từ BulletDamageSender
    private float GetBulletDamage()
    {
        if (damageSender == null) return 0f;
        return damageSender.GetDamage();
    }

    private bool IsTargetDead()
    {
        if (targetDamageReceiver == null) return true;
        return targetDamageReceiver.IsDead();
    }
}*/

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

        // Kiểm tra xem mục tiêu có còn tồn tại không
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            // Nếu không có mục tiêu, di chuyển thẳng theo hướng hiện tại
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
    }
}