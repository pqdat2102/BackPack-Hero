using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletMoveStraight : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    private StraightMove moveStrategy;
    private Transform target; // Mục tiêu (enemy)

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnEnable()
    {
        moveStrategy = new StraightMove();
    }

    private void Update()
    {
        if (transform.parent != null && target != null)
        {
            moveStrategy.Move(transform.parent, bulletSpeed, Time.deltaTime, target);
        }
    }
}
