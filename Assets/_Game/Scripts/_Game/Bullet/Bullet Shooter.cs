using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private Dice dice; // Tham chiếu đến Dice để lấy giá trị mặt xúc xắc
    [SerializeField] private float timeBetweenShoots = 0.2f; // Thời gian giữa các viên đạn
    private BulletFindTarget targetFinder; // Tham chiếu đến BulletFindTarget

    private void Start()
    {
        // Tìm Dice trong scene (hoặc gán trong Inspector)
        if (dice == null)
        {
            dice = FindObjectOfType<Dice>();
        }

        // Lấy hoặc thêm BulletFindTarget trên cùng GameObject
        targetFinder = GetComponent<BulletFindTarget>();
        if (targetFinder == null)
        {
            targetFinder = gameObject.AddComponent<BulletFindTarget>();
        }

        // Bắt đầu quy trình quay và bắn
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            // Reset danh sách assignedBullets trước khi bắt đầu lượt bắn mới
            /*targetFinder.ResetAssignedBullets();*/

            // Đảm bảo Dice có thể quay
            dice.SetCanRoll(true);

            // Gọi Dice quay
            dice.StartRoll();

            // Chờ Dice bắt đầu quay
            yield return new WaitUntil(() => dice.IsRolling());

            // Chờ Dice quay xong
            yield return new WaitUntil(() => !dice.IsRolling());

            // Lấy giá trị mặt xúc xắc
            int bulletCount = dice.GetNumberBullet(); // Số đạn từ faceTexts
            int faceValue = dice.GetFaceIndexValue(); // Giá trị mặt (1-6) để chọn loại đạn
            /*Debug.Log($"Dice: bulletCount={bulletCount}, faceValue={faceValue}");*/

            // Ngăn Dice quay trong lúc bắn
            dice.SetCanRoll(false);

            // Bắn đạn
            yield return StartCoroutine(SpawnBulletsSequentially(bulletCount, faceValue));

            // Couldow đây ( nhớ chỉnh )
            // Thêm một chút thời gian chờ để tránh lặp quá nhanh
            yield return new WaitForSeconds(0.75f);
        }
    }

    private IEnumerator SpawnBulletsSequentially(int count, int faceValue)
    {
        Vector3 spawnPosition = this.transform.position;

        // Xác định loại đạn dựa trên mặt xúc xắc (lẻ: thẳng, chẵn: cong)
        string bulletType = (faceValue % 2 != 0) ? BulletSpawner.bullet_1 : BulletSpawner.bullet_2;
        /*Debug.Log($"Bullet type: {bulletType}");*/

        for (int i = 0; i < count; i++)
        {
            // Tìm mục tiêu mới cho mỗi viên đạn trước khi bắn
            Transform bulletTarget = targetFinder.FindTarget(spawnPosition);

            // Tạo góc ban đầu cho viên đạn
            float angle = 90f; // Hướng mặc định nếu không có mục tiêu
            if (bulletTarget != null)
            {
                Vector3 directionToTarget = (bulletTarget.position - spawnPosition).normalized;
                angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            }
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Spawn đạn dựa trên loại
            Transform bullet = BulletSpawner.Instance.Spawn(bulletType, spawnPosition, rotation);
            bullet.gameObject.SetActive(true);

            // Gán mục tiêu cho viên đạn
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

            // Đợi trước khi bắn viên đạn tiếp theo
            yield return new WaitForSeconds(timeBetweenShoots);
        }
    }
}