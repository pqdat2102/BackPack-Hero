using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : DicevsMonsterMonobehavior
{
    [SerializeField] private EnemySpawner enemySpawner;
    public EnemySpawner EnemySpawner => enemySpawner;

    [SerializeField] private EnemySpawnPoints enemySpawnPoints;
    public EnemySpawnPoints EnemySpawnPoints => enemySpawnPoints;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemySpawner();
        this.LoadEnemySpawnPoints();
    }
    protected virtual void LoadEnemySpawner()
    {
        if (this.enemySpawner != null) return;
        this.enemySpawner = GetComponent<EnemySpawner>();   
        Debug.Log(transform.name + " Load EnemySpawner", gameObject);
    }

    protected virtual void LoadEnemySpawnPoints()
    {
        if (this.enemySpawnPoints != null) return;
        this.enemySpawnPoints = Transform.FindObjectOfType<EnemySpawnPoints>();
        Debug.Log(transform.name + " Load EnemySpawnPoints", gameObject);
    }
}