using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : DicevsMonsterMonobehavior
{
    [SerializeField] protected int damage = 1;

    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>(); ;
        if (damageReceiver == null) return;
        this.Send(damageReceiver);
    } 

    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.Deduct(this.damage);
    }

    // Thêm phương thức để lấy damage
    public virtual int GetDamage()
    {
        return damage;
    }

    public virtual void SetDamage(int newDamage)
    {
        this.damage = newDamage;
    }
}
