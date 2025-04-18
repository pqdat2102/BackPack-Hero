using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BulletFindTarget : DicevsMonsterMonobehavior
{
    [SerializeField] private Transform EnemyHolder;
    public float detectionRange = 10f;
    private float bulletDamage = 1f;
    private Dictionary<Transform, int> assignedBullets = new Dictionary<Transform, int>();
    private List<Transform> cachedEnemies;

    public void SetBulletDamage(float damage)
    {
        this.bulletDamage = damage;
        /*Debug.Log($"BulletFindTarget: BulletConfig set with damage: {damage}");*/
    }

    public List<Transform> FindEnemies()
    {
        List<Transform> enemies = new List<Transform>();
        foreach (Transform child in EnemyHolder)
        {
            if (child.gameObject.activeInHierarchy)
            {
                enemies.Add(child);
            }
        }

        if (enemies.Count == 0)
        {
            Debug.Log("Không có enemy nào trong EnemyHolder!");
        }

        return enemies;
    }

    public void CacheEnemies(Vector3 startPosition)
    {
        cachedEnemies = FindEnemies();
        if (cachedEnemies != null && cachedEnemies.Count > 0)
        {
            cachedEnemies = FilterAndSortEnemies(cachedEnemies, startPosition);
        }
    }

    private List<Transform> FilterAndSortEnemies(List<Transform> enemies, Vector3 startPosition)
    {
        return enemies
            .Where(enemy =>
            {
                EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
                if (damageReceiver == null || damageReceiver.IsDead()) return false;

                float distance = Vector3.Distance(startPosition, enemy.position);
                return distance <= detectionRange;
            })
            .OrderBy(enemy => enemy.position.y)
            .ToList();
    }

    private Transform FindNextTarget(float bulletDamage)
    {
        if (cachedEnemies == null || cachedEnemies.Count == 0)
        {
            Debug.Log("Không tìm thấy mục tiêu hợp lệ trong phạm vi!");
            return null;
        }

        foreach (Transform enemy in cachedEnemies)
        {
            EnemyDamageReceiver damageReceiver = enemy.GetComponentInChildren<EnemyDamageReceiver>();
            if (damageReceiver == null || damageReceiver.IsDead()) continue;

            float currentHP = damageReceiver.GetCurrentHP();
            //Debug.Log(enemy.name + " HP: " + currentHP + "/" + damageReceiver.GetMaxHP() + " - số viên đạn cần thiết: " + Mathf.CeilToInt(currentHP / bulletDamage));
            int bulletsNeeded = Mathf.CeilToInt(currentHP / bulletDamage);
            if (!assignedBullets.ContainsKey(enemy))
            {
                assignedBullets[enemy] = 0;
            }
            int bulletsAssigned = assignedBullets[enemy];
            if (bulletsAssigned >= bulletsNeeded)
            {
                continue;
            }

            assignedBullets[enemy]++;
            return enemy;
        }

        Debug.Log("Không tìm thấy mục tiêu hợp lệ để bắn! Viên đạn sẽ bay thẳng.");
        return null;
    }

    public Transform FindTarget(Vector3 startPosition)
    {
        if (bulletDamage <= 0)
        {
            Debug.LogWarning("Bullet damage not set in BulletFindTarget! Using default damage: 1");
            bulletDamage = 1f;
        }
        //Debug.Log("Tìm kiếm mục tiêu với sát thương viên đạn: " + bulletDamage);
        return FindNextTarget(bulletDamage);
    }

    public void ResetAssignedBullets()
    {
        assignedBullets.Clear();
    }
}