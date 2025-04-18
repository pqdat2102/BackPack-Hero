using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectMoveDown : DicevsMonsterMonobehavior
{
    [SerializeField] protected float moveSpeed = 1f; // Tốc độ di chuyển xuống (đơn vị/giây)
    [SerializeField] protected Vector3 direction = Vector3.down; // Hướng di chuyển (trục Y âm)

    private Rigidbody2D rb; // Rigidbody2D của object cha
    protected override void Start()
    {
        // Lấy Rigidbody 2D từ object cha (transform.parent)
        rb = transform.parent.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Khong tim thay rb trong object cha cua Enemy");
        }
    }
    private void FixedUpdate()
    {
        Vector2 moveDirection = new Vector2(direction.x, direction.y).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }
}
