using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class BulletImpact : BulletAbstract
{
    [Header("Bullet Impact")]
    [SerializeField] protected CapsuleCollider2D capsuleCollider2D  ;
    [SerializeField] protected Rigidbody2D rb2d;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
        this.LoadRigidbody();
    }

    protected virtual void LoadCollider()
    {
        if (this.capsuleCollider2D != null) return;
        this.capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        this.capsuleCollider2D.isTrigger = true;
        Debug.Log(transform.name + ": Load Collider", gameObject);
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb2d != null) return;
        this.rb2d = GetComponent<Rigidbody2D>();
        this.rb2d.gravityScale = 0f;
        Debug.Log(transform.name + ": Load Rigidbody", gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        this.bulletController.BulletDamageSender.Send(other.transform);
    }
}
