using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : DicevsMonsterMonobehavior
{
    [Header("Spawner")]
    [SerializeField] private Transform holder;

    [SerializeField] private int spawnedCount = 0;
    public int SpawnedCount => spawnedCount;

    [SerializeField] private List<Transform> prefabs;
    [SerializeField] private List<Transform> poolObjs;
    [SerializeField] private List<Transform> listEnemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPrefabs();
        this.LoadHolder();
    }

    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");

        Debug.Log(transform.name + ": Load Holder", gameObject);
    }

    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;

        Transform prefabObject = transform.Find("Prefabs");
        foreach (Transform prefab in prefabObject)
        {
            this.prefabs.Add(prefab);
            prefab.gameObject.SetActive(false);
        }
    }

    // Call By Name
    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }
        return this.Spawn(prefab, spawnPos, rotation);
    }

    // Call Prefab
    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, rotation);

        newPrefab.parent = this.holder;
        this.spawnedCount++;
        return newPrefab;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.poolObjs)
        {
            if (poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        listEnemy.Add(newPrefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    public virtual void Despawn(Transform obj)
    {
        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
        this.spawnedCount--;
    }

    public virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }
        return null;
    }

    public virtual Transform RandomPrefab()
    {
        int random = Random.Range(0, this.prefabs.Count);
        return this.prefabs[random];
    }
}