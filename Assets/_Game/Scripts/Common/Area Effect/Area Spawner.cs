using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : Spawner
{
    private static AreaSpawner instance;
    public static AreaSpawner Instance { get => instance; }

    public static string area_effect = "Area Effect";

    protected override void Awake()
    {
        base.Awake();
        if (AreaSpawner.instance != null) Debug.LogError("Only 1 AreaSpawner allow to exist");
        AreaSpawner.instance = this;
    }
}
