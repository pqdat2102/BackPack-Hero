using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPool
{
    public enum PoolName
    {
        ItemsPool = 0,
        EnemiesPool,
        FxPool
    }
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;
        public List<SpawnPool> ListPool = new List<SpawnPool>();
        Dictionary<int, SpawnPool> dicPools = new Dictionary<int, SpawnPool>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                for (int i = 0; i < ListPool.Count; i++)
                {
                    dicPools.Add(i, ListPool[i]);
                }
            }
        }

        public SpawnPool pool(PoolName _pool)
        {
            return dicPools[(int)_pool];
        }

        public void DespawnAll()
        {
            for (int i = 0; i < dicPools.Count; i++)
            {
                dicPools[i].DespawnAll();
            }
        }
    }
}
