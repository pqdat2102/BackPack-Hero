using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : DicevsMonsterMonobehavior
{
    [SerializeField] protected Transform model;
    public Transform Model { get => model; }

    [SerializeField] protected BulletDamageSender bulletDamageSender;
    public BulletDamageSender BulletDamageSender { get => bulletDamageSender; }

    [SerializeField] protected BulletDespaw bulletDespaw;
    public BulletDespaw BulletDespaw { get => bulletDespaw; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadBulletDamageSender();
        this.LoadBulletDespawn();
    }

    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
        Debug.Log(transform.name + ": LoadModel", gameObject);
    }

    protected virtual void LoadBulletDamageSender()
    {
        if (bulletDamageSender != null) return;
        bulletDamageSender = GetComponentInChildren<BulletDamageSender>();
        Debug.Log(transform.name + ": Load BulletDamageSender", gameObject);
    }

    protected virtual void LoadBulletDespawn()
    {
        if (bulletDespaw != null) return;
        bulletDespaw = GetComponentInChildren<BulletDespaw>();
        Debug.Log(transform.name + ": Load BulletDespawn", gameObject);
    }
}
