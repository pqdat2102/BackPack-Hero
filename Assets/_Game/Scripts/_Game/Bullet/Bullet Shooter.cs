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
            Debug.Log("Không tìm thấy targetFinder!");
            targetFinder = gameObject.AddComponent<BulletFindTarget>();
        }

        // Bắt đầu quy trình quay và bắn
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            /*Debug.Log("BulletShooter: Bắt đầu vòng lặp");*/

            // Reset danh sách assignedBullets trước khi bắt đầu lượt bắn mới
            targetFinder.ResetAssignedBullets();

            // Đảm bảo Dice có thể quay
            dice.SetCanRoll(true);

            // Gọi Dice quay
            /*Debug.Log("BulletShooter: Gọi Dice quay");*/
            dice.StartRoll();

            // Chờ Dice bắt đầu quay
            /*Debug.Log("BulletShooter: Chờ Dice bắt đầu quay");*/
            yield return new WaitUntil(() => dice.IsRolling());

            // Chờ Dice quay xong
            /*Debug.Log("BulletShooter: Chờ Dice quay xong");*/
            yield return new WaitUntil(() => !dice.IsRolling());

            // Lấy giá trị mặt xúc xắc
            int face = dice.GetDiceFace();
            /*Debug.Log("Bắn " + face + " viên đạn");*/

            // Ngăn Dice quay trong lúc bắn
            dice.SetCanRoll(false);

            // Bắn đạn
            /*Debug.Log("BulletShooter: Bắt đầu bắn");*/
            yield return StartCoroutine(SpawnBulletsSequentially(face));
            /*Debug.Log("BulletShooter: Bắn xong");*/

            // Thêm một chút thời gian chờ để tránh lặp quá nhanh
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnBulletsSequentially(int count)
    {
        Vector3 spawnPosition = this.transform.position;

        for (int i = 0; i < count; i++)
        {
            /*Debug.Log($"BulletShooter: Bắn viên đạn thứ {i + 1}/{count}");*/

            // Tìm mục tiêu mới cho mỗi viên đạn trước khi bắn
            Transform bulletTarget = targetFinder.FindTarget(spawnPosition);
            Debug.Log("Mục tiêu hiện tại: " + (bulletTarget != null ? bulletTarget.name : "null"));
            if (bulletTarget == null)
            {
                Debug.Log("Không có mục tiêu, viên đạn sẽ bay thẳng.");
            }

            // Tạo góc ban đầu cho viên đạn
            float angle = 90f; // Hướng mặc định nếu không có mục tiêu
            if (bulletTarget != null)
            {
                Vector3 directionToTarget = (bulletTarget.position - spawnPosition).normalized;
                angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            }
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Spawn đạn
            Transform bullet = BulletSpawner.Instance.Spawn(BulletSpawner.bullet_1, spawnPosition, rotation);
            bullet.gameObject.SetActive(true);

            // Gán mục tiêu cho viên đạn
            BulletFly bulletFly = bullet.GetComponentInChildren<BulletFly>();
            if (bulletFly != null)
            {
                bulletFly.SetTarget(bulletTarget);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy BulletFly component trên viên đạn!");
            }

            // Đợi trước khi bắn viên đạn tiếp theo
            yield return new WaitForSeconds(timeBetweenShoots);
        }
    }
}