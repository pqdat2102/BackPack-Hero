using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageSender : DamageSender
{
    [SerializeField] protected BulletController bulletController;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletController();
    }

    protected virtual void LoadBulletController()
    {
        if (this.bulletController != null) return;
        this.bulletController = transform.parent.GetComponent<BulletController>();
        Debug.Log(transform.name + ": LoadBulletController", gameObject);
    }

    public override void Send(DamageReceiver damageReceiver)
    {
        base.Send(damageReceiver);
        DestroyBullet();
    }

    public virtual void DestroyBullet()
    {
        this.bulletController.BulletDespaw.DespawnObject();
    }
}
