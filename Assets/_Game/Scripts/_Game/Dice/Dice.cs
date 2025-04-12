using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Dice : MonoBehaviour
{ 
    [SerializeField] private float rollDuration = 1.5f; // Thời gian toàn bộ hiệu ứng
    [SerializeField] private Vector3 rotationSpeed; // Tốc độ quay
    [SerializeField] private float maxHeight = 1f; // Độ cao tối đa bật lên (trục Z)
    [SerializeField] public List<TextMeshPro> faceTexts;

    private Quaternion targetRotation; // Góc quay mục tiêu khi dừng
    private bool canRoll = true; // Kiểm soát việc quay xúc xắc
    private float elapsedTime = 0f;
    private Vector3 startPos;
    private bool isRolling = false;

    private void Start()
    {
        startPos = this.transform.position;
        StartRoll(); // Quay lần đầu tiên khi game bắt đầu
    }

    private void Update()
    {
        if (isRolling)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rollDuration; // Tỷ lệ thời gian (0 -> 1)

            // Mô phỏng bật lên và rơi xuống
            float height = maxHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // Parabol
            transform.position = new Vector3(startPos.x, startPos.y + height, startPos.z);

            // Quay trong quá trình di chuyển
            if (t < 0.4f) // Quay tự do
            {
                transform.Rotate(rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (t - 0.4f) * 0.2f);
            }

            // Khi hoàn thành
            if (t >= 1f)
            {                transform.position = startPos;
                transform.rotation = targetRotation; // Đảm bảo góc tròn hoàn toàn
                isRolling = false;
                /*Debug.Log("Dice: Quay xong");*/
                ShowResult();
            }
        }
    }

    public void StartRoll()
    {
        if (!canRoll || isRolling)
        {
/*            Debug.Log($"Dice: Không thể quay - canRoll: {canRoll}, isRolling: {isRolling}");*/
            return;
        }

/*        Debug.Log("Dice: Bắt đầu quay");*/
        isRolling = true;
        elapsedTime = 0f;

        // Tốc độ quay ngẫu nhiên
        rotationSpeed = new Vector3(

         Random.Range(360f, 540f),
         Random.Range(360f, 540f),
         Random.Range(360f, 540f)
        );

        // Chọn ngẫu nhiên một mặt làm mục tiêu khi dừng
        int face = Random.Range(1, 7); // Chọn mặt từ 1 đến 6
        targetRotation = GetRotationForFace(face);
    }

    private Quaternion GetRotationForFace(int face)
    {
        switch (face)
        {
            case 1: return Quaternion.Euler(0, 0, 0);      // Mặt 1 (up = Vector3.up)
            case 2: return Quaternion.Euler(-90, 0, 0);    // Mặt 2 (-right = Vector3.up)
            case 3: return Quaternion.Euler(-180, 0, 0);   // Mặt 3 (forward = Vector3.up)
            case 4: return Quaternion.Euler(90, 0, 0);   // Mặt 4 (right = Vector3.up)
            case 5: return Quaternion.Euler(0, 90, 0);    // Mặt 5 (-forward = Vector3.up) // face 4
            case 6: return Quaternion.Euler(0, -90, 0);   // Mặt 6 (-up = Vector3.up) // face 3
            default: return Quaternion.identity;
        }
    }

    private void ShowResult()
    {
        int face = GetDiceFace();
        //Debug.Log("Giá trị xúc xắc: " + face);
    }

    public int GetDiceFace()
    {
        // Ngưỡng độ lệch tối đa (độ)
        float tolerance = 5f; // Chấp nhận sai lệch 5 độ

        // Kiểm tra góc quay hiện tại so với các góc định nghĩa
        Quaternion currentRotation = transform.rotation;

        int faceIndex = -1;

        if (Quaternion.Angle(currentRotation, GetRotationForFace(1)) < tolerance) faceIndex = 0; // Face_1
        else if (Quaternion.Angle(currentRotation, GetRotationForFace(2)) < tolerance) faceIndex = 1; // Face_2
        else if (Quaternion.Angle(currentRotation, GetRotationForFace(3)) < tolerance) faceIndex = 2; // Face_3
        else if (Quaternion.Angle(currentRotation, GetRotationForFace(4)) < tolerance) faceIndex = 3; // Face_4
        else if (Quaternion.Angle(currentRotation, GetRotationForFace(5)) < tolerance) faceIndex = 4; // Face_5
        else if (Quaternion.Angle(currentRotation, GetRotationForFace(6)) < tolerance) faceIndex = 5; // Face_6
        else
        {
            Debug.LogWarning("Không xác định được mặt, trả về mặc định");
            faceIndex = 0; // Mặc định là Face_1
        }

        // Lấy giá trị từ Text component của mặt tương ứng và ép kiểu thành int
        string textValue = faceTexts[faceIndex].text;
        int result;
        if (int.TryParse(textValue, out result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Không thể ép kiểu giá trị Text '{textValue}' thành int, trả về mặc định");
            return 1; // Mặc định trả về 1 nếu ép kiểu thất bại
        }
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void SetCanRoll(bool value)
    {
        //Debug.Log($"Dice: SetCanRoll({value})");
        canRoll = value;
    }
}