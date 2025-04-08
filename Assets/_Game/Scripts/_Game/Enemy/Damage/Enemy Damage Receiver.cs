using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    [Header("Enemy")]
    [SerializeField] protected EnemyController enemyController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyController();
    }

    protected virtual void LoadEnemyController()
    {
        if (enemyController != null) return;
        this.enemyController = transform.parent.GetComponent<EnemyController>();
        Debug.Log(transform.name + ": Loade EnemyController", gameObject);
    }

    protected override void OnDead()
    {
        //this.OnDeadFX();
        this.enemyController.EnemyDespawn.DespawnObject();
    }

   /* protected virtual void OnDeadFX()
    {
        string fxName = this.GetOnDeadFXName();
        Transform fxOnDead = FXSpawner.Instance.Spawn(fxName, transform.position, transform.rotation);
        fxOnDead.gameObject.SetActive(true);
    }

    protected virtual string GetOnDeadFXName()
    {
        return FXSpawner.smoke_1;
    }*/

    public override void Reborn()
    {
     /*   this.maxHP = this.enemyController.EnemySO.maxHP;*/
        base.Reborn();
    }
}
