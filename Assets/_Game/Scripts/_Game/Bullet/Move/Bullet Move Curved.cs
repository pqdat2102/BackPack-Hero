using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletMoveCurved : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float curvedHeight = 45f;
    private CurvedMove moveStrategy;
    private Transform target; // Mục tiêu (enemy)

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnEnable()
    {
        moveStrategy = new CurvedMove(rotationSpeed, curvedHeight);
    }

    private void Update()
    {
        if (transform.parent != null && target != null)
        {
            moveStrategy.Move(transform.parent, bulletSpeed, Time.deltaTime, target);
        }
    }
}
