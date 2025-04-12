using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    [Header("Enemy")]
    [SerializeField] protected EnemyController enemyController;

    [Header("Rewards")]
    [SerializeField] private int goldReward = 1;
    [SerializeField] private int expReward = 10;

    public bool isAlive = true;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyController();
    }

    protected virtual void LoadEnemyController()
    {
        if (enemyController != null) return;
        this.enemyController = transform.parent.GetComponent<EnemyController>();
        Debug.Log(transform.name + ": Load EnemyController", gameObject);
    }

    protected override void OnDead()
    {
        isAlive = false;
        //this.OnDeadFX();
        PlayerResources.Instance.AddGold(goldReward);
        PlayerResources.Instance.AddExperience(expReward);
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

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetCurrentHP()
    {
        return currentHP; // Giả sử DamageReceiver có biến hp (HP hiện tại)
    }
}
