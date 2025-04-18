using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : DicevsMonsterMonobehavior
{
    [SerializeField] protected Transform model;
    public Transform Model { get => model; }

    [SerializeField] protected EnemyDespawn enemyDespawn;
    public EnemyDespawn EnemyDespawn { get => enemyDespawn; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadEnemyDespawn();

    }

    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
        Debug.Log(transform.name + ": LoadModel", gameObject);
    }

    protected virtual void LoadEnemyDespawn()
    {
        if (enemyDespawn != null) return;
        this.enemyDespawn = GetComponentInChildren<EnemyDespawn>();
        Debug.Log(transform.name + ": Load EnemyDespawn", gameObject);
    }
}

