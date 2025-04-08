using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BulletAbstract : DicevsMonsterMonobehavior
{
    [Header("Bullet Abstract")]
    [SerializeField] protected BulletController bulletController;
    public BulletController BulletController { get => bulletController; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageReveiver();
    }

    protected virtual void LoadDamageReveiver()
    {
        if (this.bulletController != null) return;
        this.bulletController = transform.parent.GetComponent<BulletController>();
        Debug.Log(transform.name + ": LoadDamageReveiver", gameObject);
    }
}
