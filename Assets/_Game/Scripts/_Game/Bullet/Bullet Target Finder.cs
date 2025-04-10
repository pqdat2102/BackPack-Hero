using UnityEngine;

public class BulletTargetFinder : DicevsMonsterMonobehavior
{
    public float detectionRange = 10f; // Phạm vi tìm enemy
    private BulletDamageSender damageSender; // Tham chiếu đến BulletDamageSender để lấy sát thương

   /* private void Awake()
    {
        // Lấy BulletDamageSender từ transform.parent (Bullet)
        damageSender = BulletController.BulletDamageSender;
        if (damageSender == null)
        {
            Debug.LogError("BulletDamageSender not found on parent of " + gameObject.name, gameObject);
        }
    }*/

    // Tìm mục tiêu cho viên đạn
    public Transform FindTarget(Vector3 startPosition)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        float bulletDamage = GetBulletDamage(); // Lấy sát thương của viên đạn

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
            if (damageReceiver == null || damageReceiver.IsDead()) continue;

            // Bỏ qua enemy nếu HP của nó nhỏ hơn hoặc bằng sát thương của viên đạn
            if (bulletDamage > 0 && damageReceiver.GetCurrentHP() <= bulletDamage) continue;

            float distance = Vector3.Distance(startPosition, enemy.transform.position);
            if (distance < minDistance && distance <= detectionRange)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        // Nếu không tìm thấy mục tiêu hợp lệ, trả về null
        if (closestEnemy == null)
        {
            Debug.Log("Không tìm thấy mục tiêu hợp lệ để bắn!");
            return null;
        }

        return closestEnemy;
    }

    // Lấy sát thương từ BulletDamageSender
    private float GetBulletDamage()
    {
        if (damageSender == null) return 0f;
        return 1;
    }
}