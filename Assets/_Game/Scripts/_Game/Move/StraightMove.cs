using UnityEngine;

public class StraightMove : IMove
{
    private Transform target;

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

        bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, target.position, speed * deltaTime);
    }
} 