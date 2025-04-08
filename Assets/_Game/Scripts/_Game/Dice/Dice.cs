using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    private bool isRolling = false;
    [SerializeField] private float rollDuration = 1.5f; // Thời gian toàn bộ hiệu ứng
    private float elapsedTime = 0f;
    private Vector3 startPos;
    [SerializeField] private Vector3 rotationSpeed; // Tốc độ quay
    [SerializeField] private float maxHeight = 2f; // Độ cao tối đa bật lên (trục Z)
    [SerializeField] private Quaternion targetRotation; // Góc quay mục tiêu khi dừng

    private void Start()
    {
        startPos = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            StartRolling();
        }

        if (isRolling)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rollDuration; // Tỷ lệ thời gian (0 -> 1)

            // Mô phỏng bật lên và rơi xuống
            float height = maxHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // Parabol
            transform.position = new Vector3(startPos.x, startPos.y, startPos.z + height);

            // Quay trong quá trình di chuyển
            if (t < 0.8f) // Quay tự do trong 80% thời gian
            {
                transform.Rotate(rotationSpeed * Time.deltaTime);
            }
            else // Căn chỉnh dần về góc tròn trong 20% cuối
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (t - 0.8f) * 5f);
            }

            // Khi hoàn thành
            if (t >= 1f)
            {
                transform.position = startPos;
                transform.rotation = targetRotation; // Đảm bảo góc tròn hoàn toàn
                isRolling = false;
                ShowResult();
            }
        }
    }

    void StartRolling()
    {
        isRolling = true;
        elapsedTime = 0f;

        // Tốc độ quay ngẫu nhiên
        rotationSpeed = new Vector3(
            Random.Range(360f, 720f),
            Random.Range(360f, 720f),
            Random.Range(360f, 720f)
        );

        // Chọn ngẫu nhiên một mặt làm mục tiêu khi dừng
        int face = Random.Range(1, 7); // Chọn mặt từ 1 đến 6
        targetRotation = GetRotationForFace(face);
    }

    Quaternion GetRotationForFace(int face)
    {
        // Định nghĩa góc quay theo yêu cầu của bạn
        switch (face)
        {
            case 1: return Quaternion.Euler(0, 0, 0);      // Mặt 6 (up = Vector3.up)
            case 6: return Quaternion.Euler(180, 0, 0);   // Mặt 1 (-up = Vector3.up)
            case 2: return Quaternion.Euler(-90, 0, 0);   // Mặt 2 (forward = Vector3.up)
            case 5: return Quaternion.Euler(90, 0, 0);    // Mặt 5 (-forward = Vector3.up)
            case 3: return Quaternion.Euler(0, -90, 0);   // Mặt 3 (right = Vector3.up)
            case 4: return Quaternion.Euler(0, 90, 0);    // Mặt 4 (-right = Vector3.up)
            default: return Quaternion.identity;
        }
    }

    void ShowResult()
    {
        int face = GetDiceFace();
        Debug.Log("Giá trị xúc xắc: " + face);
        StartCoroutine(SpawnBulletsSequentially(face));
    }

    IEnumerator SpawnBulletsSequentially(int count)
    {
        // Vị trí spawn đạn (có thể điều chỉnh theo ý muốn)
        Vector3 spawnPosition = this.transform.position;
        
        // Spawn số lượng đạn tương ứng với mặt xúc xắc, mỗi viên cách nhau 0.2 giây
        for (int i = 0; i < count; i++)
        {
            // Tạo góc ngẫu nhiên cho mỗi viên đạn
            float angle = 90f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            
            // Spawn đạn thông qua BulletSpawner
            Transform bullet = BulletSpawner.Instance.Spawn(BulletSpawner.bullet_1, spawnPosition, rotation);
            if (bullet != null)
            {
                bullet.gameObject.SetActive(true);
            }
            
            // Đợi 0.2 giây trước khi spawn viên đạn tiếp theo
            yield return new WaitForSeconds(0.5f);
        }
    }

    int GetDiceFace()
    {
        // Ngưỡng độ lệch tối đa (độ)
        float tolerance = 5f; // Chấp nhận sai lệch 5 độ

        // Kiểm tra góc quay hiện tại so với các góc định nghĩa
        Quaternion currentRotation = transform.rotation;

        if (Quaternion.Angle(currentRotation, GetRotationForFace(1)) < tolerance) return 6; // Mặt 6
        if (Quaternion.Angle(currentRotation, GetRotationForFace(6)) < tolerance) return 1; // Mặt 1
        if (Quaternion.Angle(currentRotation, GetRotationForFace(2)) < tolerance) return 2; // Mặt 2
        if (Quaternion.Angle(currentRotation, GetRotationForFace(5)) < tolerance) return 5; // Mặt 5
        if (Quaternion.Angle(currentRotation, GetRotationForFace(3)) < tolerance) return 3; // Mặt 3
        if (Quaternion.Angle(currentRotation, GetRotationForFace(4)) < tolerance) return 4; // Mặt 4

        // Dự phòng: Nếu không khớp, trả về mặt 1 (nên không xảy ra vì targetRotation luôn khớp)
        Debug.LogWarning("Không xác định được mặt, trả về mặc định");
        return 1;
    }
}