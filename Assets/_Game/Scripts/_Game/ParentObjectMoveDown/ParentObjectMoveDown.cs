using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectMoveDown : DicevsMonsterMonobehavior
{
    [SerializeField] protected float moveSpeed = 1f; // Tốc độ di chuyển xuống (đơn vị/giây)
    [SerializeField] protected Vector3 direction = Vector3.down; // Hướng di chuyển (trục Y âm)

    private void Update()
    {
        transform.parent.Translate(this.direction * this.moveSpeed * Time.deltaTime);
    }
}
