// BulletDamageSender.cs
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BulletDamageSender : DamageSender
{
    [SerializeField] protected BulletController bulletController;
    private List<IBulletEffect> effects = new List<IBulletEffect>();

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

    protected override void OnEnable()
    {
        // Khởi tạo danh sách hiệu ứng
        effects.Add(new AreaEffect(bulletController.AreaController));
        // Thêm các hiệu ứng khác ở đây nếu cần
    }

    public override void Send(DamageReceiver damageReceiver)
    {
        base.Send(damageReceiver);
        DestroyBullet();
        ApplyEffects(damageReceiver.transform);
    }

    protected virtual void ApplyEffects(Transform target)
    {
        BulletData data = bulletController.GetData();
        if (data != null)
        {
            foreach (var effect in effects)
            {
                effect.ApplyEffect(target, data);
            }
        }
    }
    public virtual void DestroyBullet()
    {
        this.bulletController.BulletDespaw.DespawnObject();
    }

}