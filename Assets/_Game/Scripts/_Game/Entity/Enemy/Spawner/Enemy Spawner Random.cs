using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnerRandom : DicevsMonsterMonobehavior
{
    [Header("Enemy Spawner Random")]
    [SerializeField] protected EnemySpawnerController enemySpawnerController;
    [SerializeField] protected float randomDelay = 1f;
    [SerializeField] protected float randomTimer = 0f;
    [SerializeField] protected float randomCountLimit = 20f;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyController();
    }

    protected virtual void LoadEnemyController()
    {
        if (this.enemySpawnerController != null) return;
        this.enemySpawnerController = GetComponent<EnemySpawnerController>();
        Debug.Log(transform.name + ": Load EnemyController", gameObject);
    }

    protected virtual void FixedUpdate()
    {
        this.EnemySpawning();
    }

    protected virtual void EnemySpawning()
    {
        if (this.RandomReachLimit()) return;

        // Delay
        this.randomTimer += Time.fixedDeltaTime;
        if (this.randomTimer < this.randomDelay) return;
        this.randomTimer = 0f;

        // Spawn Enemy
        Transform randomPoint = this.enemySpawnerController.EnemySpawnPoints.GetRandom();
        Vector3 pos = randomPoint.position;
        Quaternion rot = transform.rotation;

        Transform prefab = this.enemySpawnerController.EnemySpawner.RandomPrefab();
        Transform obj = this.enemySpawnerController.EnemySpawner.Spawn(prefab, pos, rot);
        obj.gameObject.SetActive(true);
    }

    protected virtual bool RandomReachLimit()
    {
        int currentJunk = this.enemySpawnerController.EnemySpawner.SpawnedCount;
        return currentJunk >= this.randomCountLimit;
    }
}
