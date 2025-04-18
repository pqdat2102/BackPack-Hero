using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemySpawner instance;

    public static EnemySpawner Instance { get => instance; }

    public static string monster_1 = "Monster_1";   

    protected override void Awake()
    {
        base.Awake();
        if(EnemySpawner.instance != null) Debug.LogError("EnemySpawner already exists");
        EnemySpawner.instance = this;
    }
}
