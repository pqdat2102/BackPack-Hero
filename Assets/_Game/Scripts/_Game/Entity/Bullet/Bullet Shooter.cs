using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float timeBetweenShoots = 0.2f;
    [SerializeField] private BulletFindTarget targetFinder;
    [SerializeField] private BulletConfigManager configManager; // Thay BulletConfigList bằng BulletConfigManager

    public IEnumerator SpawnBulletsSequentially(int count, int faceValue)
    {
        Vector3 spawnPosition = transform.position;
        string bulletType = (faceValue % 2 != 0) ? BulletSpawner.bullet_1 : BulletSpawner.bullet_2;

        // Lấy BulletData từ BulletConfigManager
        BulletData bulletData = configManager.GetBulletData(bulletType);
        if (bulletData == null)
        {
            Debug.LogError($"BulletData for {bulletType} not found in BulletConfigManager!");
            yield break;
        }

        for (int i = 0; i < count; i++)
        {
            // Truyền sát thương vào BulletFindTarget
            targetFinder.SetBulletDamage(bulletData.damage);

            // Tìm mục tiêu
            Transform bulletTarget = targetFinder.FindTarget(spawnPosition);

            float angle = 90f;
            if (bulletTarget != null)
            {
                Vector3 directionToTarget = (bulletTarget.position - spawnPosition).normalized;
                angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            }
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Transform bullet = BulletSpawner.Instance.Spawn(bulletType, spawnPosition, rotation);
            if (bullet != null)
            {
                bullet.gameObject.SetActive(true);

                // Gán BulletData cho viên đạn vừa spawn
                BulletController bulletController = bullet.GetComponent<BulletController>();
                if (bulletController != null)
                {
                    bulletController.SetData(bulletData);
                }

                if (bulletType == BulletSpawner.bullet_1)
                {
                    BulletMoveStraight bulletMoveStraight = bullet.GetComponentInChildren<BulletMoveStraight>();
                    if (bulletMoveStraight != null)
                    {
                        if (bulletTarget != null)
                        {
                            bulletMoveStraight.SetTarget(bulletTarget);
                            bulletMoveStraight.SetBulletMove(bulletData);
                        }
                        else
                        {
                            BulletSpawner.Instance.Despawn(bullet);
                            Debug.Log("Không tìm thấy mục tiêu cho viên đạn thẳng, đã hủy viên đạn!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Không tìm thấy BulletMoveStraight component trên viên đạn!");
                    }
                }
                else
                {
                    BulletMoveCurved bulletMoveCurved = bullet.GetComponentInChildren<BulletMoveCurved>();
                    if (bulletMoveCurved != null)
                    {
                        if (bulletTarget != null)
                        {
                            bulletMoveCurved.SetTarget(bulletTarget);
                            bulletMoveCurved.SetBulletMove(bulletData);
                        }
                        else
                        {
                            BulletSpawner.Instance.Despawn(bullet);
                            Debug.Log("Không tìm thấy mục tiêu cho viên đạn cong, đã hủy viên đạn!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Không tìm thấy BulletMoveCurved component trên viên đạn!");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Failed to spawn bullet of type: {bulletType}");
            }

            yield return new WaitForSeconds(timeBetweenShoots);
        }
    }
}