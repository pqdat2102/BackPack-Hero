using UnityEngine;

public class CurvedMove : IMove
{
    private Transform target;
    private float rotationSpeed = 5f;
    private float curveStrength = 0.5f;
    private float time = 0f;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Move(Transform bulletTransform, float speed, float deltaTime)
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            bulletTransform.position += bulletTransform.right * speed * deltaTime;
            return;
        }

        time += deltaTime;
        
        // Calculate direction to target
        Vector3 directionToTarget = (target.position - bulletTransform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Add curve effect
        float curveAngle = Mathf.Sin(time * curveStrength) * 30f;
        targetAngle += curveAngle;

        // Rotate towards target with curve
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        bulletTransform.rotation = Quaternion.Lerp(bulletTransform.rotation, targetRotation, rotationSpeed * deltaTime);

        // Move forward
        bulletTransform.position += bulletTransform.right * speed * deltaTime;
    }
} 