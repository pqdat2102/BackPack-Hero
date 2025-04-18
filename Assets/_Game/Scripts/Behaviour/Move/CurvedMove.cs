using UnityEngine;

public class CurvedMove : IMove
{
    private float rotationSpeed;
    private float curveHeight;
    private bool isInitialized = false;
    private Vector3 startPosition;
    private Vector3 controlPoint;
    private Vector3 endPosition;
    private float t; // Tham số thời gian (t từ 0 đến 1)

    public CurvedMove(float rotationSpeed, float curveHeight)
    {
        this.rotationSpeed = rotationSpeed;
        this.curveHeight = Random.Range(1, curveHeight);
    }

    public void Move(Transform objTransform, float speed, float deltaTime, Transform target)
    {

        // Không có target thì bay thẳng
        if (target == null)
        {
            objTransform.position += objTransform.right * speed * deltaTime;
            Debug.Log("bay cong khong co target, di chuyen thanh 1 duong thang");
            return;
        }

        // Khởi tạo quỹ đạo cong
        if (!isInitialized)
        {
            startPosition = objTransform.position;
            endPosition = target.position;
            t = 0f;

            // Tính điểm điều khiển để tạo vòng cung trái/phải
            Vector3 directionToTarget = (endPosition - startPosition).normalized;
            // Xác định hướng cong (trái/phải) dựa trên vị trí
            Vector3 perpendicular = Vector3.Cross(directionToTarget, Vector3.forward).normalized;
            // Nếu enemy ở bên phải, cong sang phải; nếu ở bên trái, cong sang trái
            float curveDirection = (endPosition.x > startPosition.x) ? 1f : -1f;
            controlPoint = (startPosition + endPosition) / 2f + perpendicular * curveHeight * curveDirection;

            isInitialized = true;
        }

        // Cập nhật endPosition liên tục để theo target
        endPosition = target.position;

        // Tính khoảng cách cần di chuyển trong frame này
        float distanceToTarget = Vector3.Distance(startPosition, endPosition);
        if (distanceToTarget <= 0.01f) // Tránh chia cho 0
        {
            objTransform.position = endPosition;
            return;
        }

        float moveDistance = speed * deltaTime;
        t += moveDistance / distanceToTarget;

        // Nếu t vượt quá 1, đặt vị trí tại mục tiêu và dừng
        if (t >= 1f)
        {
            objTransform.position = endPosition;
            return;
        }

        // Tính vị trí mới trên đường cong Bezier bậc hai
        Vector3 position = Mathf.Pow(1 - t, 2) * startPosition +
                          2 * (1 - t) * t * controlPoint +
                          Mathf.Pow(t, 2) * endPosition;
        objTransform.position = position;

        // Cập nhật hướng của đạn
        Vector3 direction = (endPosition - objTransform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        objTransform.rotation = Quaternion.Lerp(objTransform.rotation, targetRotation, rotationSpeed * deltaTime);
    }
}