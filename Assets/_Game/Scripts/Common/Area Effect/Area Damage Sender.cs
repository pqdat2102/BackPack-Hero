using System.Collections;
using UnityEngine;

public class AreaDamageSender : DamageSender
{
    [SerializeField] protected AreaController areaController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAreaController();
    }

    protected virtual void LoadAreaController()
    {
        if (this.areaController != null) return;
        this.areaController = transform.parent.GetComponent<AreaController>();
        Debug.Log(transform.name + ": LoadAreaController", gameObject);
    }

    public void SetDamageFromConfig(float damage)
    {
        SetDamage((int)damage);
    }

    public override void Send(DamageReceiver damageReceiver)
    {
        base.Send(damageReceiver);
    }

    public virtual void DespawnArea()
    {
        this.areaController.AreaDespawn.DespawnObject();
    }
}