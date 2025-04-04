using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyPool
{
    public class SpawnPool : MonoBehaviour
    {
        public Transform group { get; private set; }
        //public string poolName = string.Empty;
        //public bool matchPoolScale;
        //public bool matchPoolLayer;
        //public bool dontReparent;
        //public bool logMessages;
        public List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();
        private List<PrefabPool> _prefabPools = new List<PrefabPool>();
        public PrefabsDict prefabs = new PrefabsDict();

        private void Awake()
        {
            Initialize();
        }
        public void Initialize()
        {
            group = transform;
            for (int i = 0; i < _perPrefabPoolOptions.Count; i++)
            {
                if (_perPrefabPoolOptions[i].prefab == null)
                {
#if ENABLE_LOG_SPAWNPOOL
				//Debug.LogWarning($"Initialization Warning: Pool '{poolName}' contains a PrefabPool with no prefab reference. Skipping.");
#endif
                    continue;
                }
                _perPrefabPoolOptions[i].inspectorInstanceConstructor();
                CreatePrefabPool(_perPrefabPoolOptions[i]);
            }
        }

        private void OnDestroy()
        {
            destroypool();
        }

        public void destroypool()
        {
#if ENABLE_LOG_SPAWNPOOL
		if (logMessages)
		{
			//Debug.Log($"SpawnPool {poolName}: Destroying...");
		}
#endif
            StopAllCoroutines();
            foreach (PrefabPool prefabPool in _prefabPools)
            {
                prefabPool.SelfDestruct();
            }
            _prefabPools.Clear();
            prefabs._Clear();
        }

        public void CreatePrefabPool(PrefabPool prefabPool)
        {
            if (GetPrefabPool(prefabPool.prefab) == null)
            {
                prefabPool.spawnPool = this;
                _prefabPools.Add(prefabPool);
                prefabs._Add(prefabPool.prefab.name, prefabPool.prefab);
            }
            if (!prefabPool.preloaded)
            {
#if ENABLE_LOG_SPAWNPOOL
			if (logMessages)
			{
				//Debug.Log($"SpawnPool {poolName}: Preloading {prefabPool.preloadAmount} {prefabPool.prefab.name}");
			}
#endif
                prefabPool.PreloadInstances();
            }
        }

        private Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform transform;
            for (int i = 0; i < _prefabPools.Count; i++)
            {
                if (_prefabPools[i].prefab.gameObject == prefab.gameObject)
                {
                    transform = _prefabPools[i].SpawnInstance(pos, rot);
                    if (transform == null)
                    {
                        return null;
                    }
                    if (parent != null)
                    {
                        transform.parent = parent;
                    }
                    else if (transform.parent != group)
                    {
                        transform.parent = group;
                    }
                    transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
                    return transform;
                }
            }
            PrefabPool prefabPool = new PrefabPool(prefab);
            CreatePrefabPool(prefabPool);
            transform = prefabPool.SpawnInstance(pos, rot);
            if (parent != null)
            {
                transform.parent = parent;
            }
            else
            {
                transform.parent = group;
            }
            transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
            return transform;
        }

        public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            Transform prefab = prefabs[prefabName];
            return Spawn(prefab, pos, rot, parent);
        }

        public void DespawnAll()
        {
            for (int i = 0; i < _prefabPools.Count; i++)
            {
                _prefabPools[i].DespawnAll();
            }
        }
        public void Despawn(Transform instance)
        {
            bool flag = false;
            for (int i = 0; i < _prefabPools.Count; i++)
            {
                if (_prefabPools[i]._spawned.Contains(instance))
                {
                    flag = _prefabPools[i].DespawnInstance(instance);
                    break;
                }
                if (_prefabPools[i]._despawned.Contains(instance))
                {
#if ENABLE_LOG_SPAWNPOOL
				//Debug.LogError($"SpawnPool {poolName}: {instance.name} has already been despawned. You cannot despawn something more than once!");
#endif
                    return;
                }
            }
            if (!flag)
            {
#if ENABLE_LOG_SPAWNPOOL
			//Debug.LogError($"SpawnPool {poolName}: {instance.name} not found in SpawnPool");
#endif
            }
        }

        public void Despawn(Transform instance, Transform parent)
        {
            instance.parent = parent;
            Despawn(instance);
        }

        public void Despawn(Transform instance, float seconds)
        {
            StartCoroutine(DoDespawnAfterSeconds(instance, seconds, useParent: false, null));
        }

        private IEnumerator DoDespawnAfterSeconds(Transform instance, float seconds, bool useParent, Transform parent)
        {
            GameObject go = instance.gameObject;
            while (seconds > 0f)
            {
                yield return null;
                if (!go.activeInHierarchy)
                {
                    yield break;
                }
                seconds -= Time.deltaTime;
            }
            if (useParent)
            {
                Despawn(instance, parent);
            }
            else
            {
                Despawn(instance);
            }
        }

        public PrefabPool GetPrefabPool(Transform prefab)
        {
            for (int i = 0; i < _prefabPools.Count; i++)
            {
                if (_prefabPools[i].prefab.gameObject == null)
                {
#if ENABLE_LOG_SPAWNPOOL
				//Debug.LogError($"SpawnPool {poolName}: PrefabPool.prefab gameobject is null");
#endif
                }
                if (_prefabPools[i].prefab.gameObject == prefab.gameObject)
                {
                    return _prefabPools[i];
                }
            }
            return null;
        }
    }
}