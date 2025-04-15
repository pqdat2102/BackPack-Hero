using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private float timeBetweenShoots = 0.2f;
    private BulletFindTarget targetFinder;

    private void Start()
    {
        if (dice == null)
        {
            dice = FindObjectOfType<Dice>();
        }

        targetFinder = GetComponent<BulletFindTarget>();
        if (targetFinder == null)
        {
            targetFinder = gameObject.AddComponent<BulletFindTarget>();
        }

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            targetFinder.ResetAssignedBullets();

            dice.SetCanRoll(true);
            dice.StartRoll();

            yield return new WaitUntil(() => dice.IsRolling());
            yield return new WaitUntil(() => !dice.IsRolling());

            int bulletCount = dice.GetNumberBullet();
            int faceValue = dice.GetFaceIndexValue();
            /*Debug.Log($"Dice: bulletCount={bulletCount}, faceValue={faceValue}");*/

            dice.SetCanRoll(false);

            // Thu thập danh sách quái một lần trước khi bắn
            targetFinder.CacheEnemies(transform.position);

            yield return StartCoroutine(SpawnBulletsSequentially(bulletCount, faceValue));

            yield return new WaitForSeconds(0.75f);
        }
    }

    private IEnumerator SpawnBulletsSequentially(int count, int faceValue)
    {
        Vector3 spawnPosition = transform.position;
        string bulletType = (faceValue % 2 != 0) ? BulletSpawner.bullet_1 : BulletSpawner.bullet_2;

        for (int i = 0; i < count; i++)
        {
            // Gọi FindTarget cho từng viên đạn, sử dụng danh sách quái đã lưu
            Transform bulletTarget = targetFinder.FindTarget(spawnPosition);

            float angle = 90f;
            if (bulletTarget != null)
            {
                Vector3 directionToTarget = (bulletTarget.position - spawnPosition).normalized;
                angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            }
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Transform bullet = BulletSpawner.Instance.Spawn(bulletType, spawnPosition, rotation);
            bullet.gameObject.SetActive(true);

            if (bulletType == BulletSpawner.bullet_1)
            {
                BulletMoveStraight bulletMoveStraight = bullet.GetComponentInChildren<BulletMoveStraight>();
                if (bulletMoveStraight != null)
                {
                    if (bulletTarget != null)
                    {
                        bulletMoveStraight.SetTarget(bulletTarget);
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

            yield return new WaitForSeconds(timeBetweenShoots);
        }
    }
}