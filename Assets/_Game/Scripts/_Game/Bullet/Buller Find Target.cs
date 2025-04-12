using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BulletFindTarget : DicevsMonsterMonobehavior
{
    [SerializeField] private Transform EnemyHolder;
    public float detectionRange = 10f; // Phạm vi tìm enemy
    private BulletDamageSender damageSender; // Tham chiếu đến BulletDamageSender để lấy sát thương
    private Dictionary<Transform, int> assignedBullets = new Dictionary<Transform, int>(); // Theo dõi số viên đạn phân bổ cho mỗi quái

    public Transform FindTarget(Vector3 startPosition)
    {
        // Lấy tất cả enemy từ EnemyHolder
        List<Transform> enemies = new List<Transform>();
        foreach (Transform child in EnemyHolder)
        {
            if (child.gameObject.activeInHierarchy) // Chỉ lấy enemy đang active
            {
                enemies.Add(child);
            }
        }

        // Nếu không có enemy nào, trả về null
        if (enemies.Count == 0)
        {
            Debug.Log("Không có enemy nào trong EnemyHolder!");
            return null;
        }

        float bulletDamage = GetBulletDamage(); // Lấy sát thương của viên đạn
        Debug.Log("Sát thương viên đạn: " + bulletDamage);

        // Sắp xếp enemies theo position.y (tăng dần)
        var sortedEnemies = enemies
            .Where(enemy =>
            {
                // Lọc enemy: chỉ lấy những enemy hợp lệ
                EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
                if (damageReceiver == null || damageReceiver.IsDead()) return false;

                // Chỉ lấy enemy trong phạm vi detectionRange
                float distance = Vector3.Distance(startPosition, enemy.position);
                return distance <= detectionRange;
            })
            .OrderBy(enemy => enemy.position.y) // Sắp xếp theo position.y
            .ToList();

        // Nếu không có enemy nào trong phạm vi, trả về null
        if (sortedEnemies.Count == 0)
        {
            Debug.Log("Không tìm thấy mục tiêu hợp lệ trong phạm vi!");
            return null;
        }

        // Duyệt qua danh sách đã sắp xếp để tìm mục tiêu
        foreach (Transform enemy in sortedEnemies)
        {
            EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
            float currentHP = damageReceiver.GetCurrentHP();
            Debug.Log("Enemy: " + enemy.name + " HP: " + currentHP + " Position.y: " + enemy.position.y);

            // Tính số viên đạn cần thiết để tiêu diệt quái
            int bulletsNeeded = Mathf.CeilToInt(currentHP / bulletDamage); // Làm tròn lên

            // Kiểm tra số viên đạn đã phân bổ cho quái này
            if (!assignedBullets.ContainsKey(enemy))
            {
                assignedBullets[enemy] = 0; // Khởi tạo nếu chưa có
            }

            int bulletsAssigned = assignedBullets[enemy];

            // Nếu quái đã nhận đủ số viên đạn để tiêu diệt, bỏ qua
            if (bulletsAssigned >= bulletsNeeded)
            {
                Debug.Log($"Bỏ qua {enemy.name} vì đã nhận đủ {bulletsAssigned}/{bulletsNeeded} viên đạn.");
                continue;
            }

            // Phân bổ thêm 1 viên đạn cho quái này
            assignedBullets[enemy]++;
            Debug.Log($"Phân bổ viên đạn cho {enemy.name}. Tổng số viên đạn: {assignedBullets[enemy]}/{bulletsNeeded}");

            // Trả về quái làm mục tiêu
            Debug.Log($"Chọn mục tiêu: {enemy.name}");
            return enemy;
        }

        // Nếu không tìm thấy mục tiêu hợp lệ, trả về null
        Debug.Log("Không tìm thấy mục tiêu hợp lệ để bắn! Viên đạn sẽ bay thẳng.");
        return null;
    }

    // Reset danh sách assignedBullets (gọi khi bắt đầu một lượt bắn mới)
    public void ResetAssignedBullets()
    {
        assignedBullets.Clear();
        Debug.Log("Đã reset danh sách assignedBullets.");
    }

    // Lấy sát thương từ BulletDamageSender
    private float GetBulletDamage()
    {
        if (damageSender == null)
        {
            damageSender = GetComponent<BulletDamageSender>();
            if (damageSender == null) return 1f;
        }
        return damageSender.GetDamage();
    }
}