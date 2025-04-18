using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class DamageReceiver : DicevsMonsterMonobehavior
{
    [SerializeField] protected CircleCollider2D circleCollider2D;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int maxHP = 3;
    [SerializeField] protected bool isDead = false;

    protected override void OnEnable()
    {
        this.Reborn();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        this.Reborn();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
    }

    protected virtual void LoadCollider()
    {
        if (this.circleCollider2D != null) return;
        this.circleCollider2D = GetComponent<CircleCollider2D>();
        this.circleCollider2D.isTrigger = true;
        Debug.Log(transform.name + ": Load Collider", gameObject);
    }

    public virtual void Reborn()
    {
        this.currentHP = this.maxHP;
        isDead = false;
    }

    public virtual void Add(int add)
    {
        if (isDead) return;

        this.currentHP += add;
        if (this.currentHP > this.maxHP) this.currentHP = this.maxHP;
    }

    public virtual void Deduct(int deduct)
    {
        this.currentHP -= deduct;
        if (this.currentHP < 0) this.currentHP = 0;
        this.CheckIsDead();
    }

    public virtual bool IsDead()
    {
        return this.currentHP <= 0;
    }

    protected virtual void CheckIsDead()
    {
        if (!this.IsDead()) return;
        this.isDead = true;
        this.OnDead();
    }

    protected abstract void OnDead();

}
