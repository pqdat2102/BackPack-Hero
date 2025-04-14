using UnityEngine;

public interface IMove
{
    void Move(Transform bulletTransform, float speed, float deltaTime);
    void SetTarget(Transform target);
} 