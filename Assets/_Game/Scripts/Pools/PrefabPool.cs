//#define ENABLE_LOG_PREFABPOOL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyPool
{

    [Serializable]
    public class PrefabPool
    {
        public Transform prefab;

        //public int preloadAmount = 1;

        //public bool preloadTime;

        //public int preloadFrames = 2;

        //public float preloadDelay;

        //public bool limitInstances;

        //public int limitAmount = 100;

        //public bool limitFIFO;

        //public bool cullDespawned;

        //public int cullAbove = 50;

        //public int cullDelay = 60;

        //public int cullMaxPerPass = 5;

        //public bool _logMessages;

        //private bool forceLoggingSilent;

        public SpawnPool spawnPool;

        private bool cullingActive;

        internal HashSet<Transform> _spawned = new HashSet<Transform>();

        internal HashSet<Transform> _despawned = new HashSet<Transform>();

        private bool _preloaded;

        public int totalCount
        {
            get
            {
                int num = 0;
                num += _spawned.Count;
                return num + _despawned.Count;
            }
        }

        internal bool preloaded
        {
            get
            {
                return _preloaded;
            }
            private set
            {
                _preloaded = value;
            }
        }
        public bool logMessages
        {
            get
            {
                //if (forceLoggingSilent)
                //{
                //    return false;
                //}
                //if (spawnPool.logMessages)
                //{
                    //return spawnPool.logMessages;
                //}
                return false;
            }
        }

        public PrefabPool(Transform prefab)
        {
            this.prefab = prefab;
        }

        internal void inspectorInstanceConstructor()
        {
            _spawned = new HashSet<Transform>();
            _despawned = new HashSet<Transform>();
        }

        internal void SelfDestruct()
        {
            //prefab = null;
            foreach (var item in _despawned)
            {
                if (item != null)
                {
                    UnityEngine.Object.Destroy(item.gameObject);
                }
            }
            foreach (var item2 in _spawned)
            {
                if (item2 != null)
                {
                    UnityEngine.Object.Destroy(item2.gameObject);
                }
            }
            _spawned.Clear();
            _despawned.Clear();
        }

        internal bool DespawnInstance(Transform xform)
        {
            return DespawnInstance(xform, sendEventMessage: true);
        }

        internal void DespawnAll()
        {
            foreach (var item in _spawned)
            {
                _despawned.Add(item);
                item.gameObject.BroadcastMessage("OnDespawned", null, SendMessageOptions.DontRequireReceiver);
                if (item.gameObject.activeSelf)
                {
                    PoolUtils.SetActive(item.gameObject, state: false);
                }
            }
            _spawned.Clear();
        }

        internal bool DespawnInstance(Transform xform, bool sendEventMessage)
        {
#if ENABLE_LOG_PREFABPOOL
		if (logMessages)
		{
			//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): Despawning '{xform.name}'");
		}
#endif
            _spawned.Remove(xform);
            _despawned.Add(xform);
            if (sendEventMessage)
            {
                xform.gameObject.BroadcastMessage("OnDespawned", null, SendMessageOptions.DontRequireReceiver);
            }

            if (xform.gameObject.activeSelf)
            {
                PoolUtils.SetActive(xform.gameObject, state: false);
            }
            //if (!cullingActive && cullDespawned && totalCount > cullAbove)
            //{
            //    cullingActive = true;
            //    spawnPool.StartCoroutine(CullDespawned());
            //}
            return true;
        }


        internal IEnumerator CullDespawned()
        {
#if ENABLE_LOG_PREFABPOOL
		if (logMessages)
		{
			//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): CULLING TRIGGERED! Waiting {cullDelay}sec to begin checking for despawns...");
		}
#endif
            //yield return new WaitForSeconds(cullDelay);
            while (totalCount > 0)
            {
                for (int i = 0; i < 0; i++)
                {
                    if (totalCount <= 0)
                    {
                        break;
                    }
                    if (_despawned.Count > 0)
                    {
                        var transform = _despawned.First();
                        _despawned.Remove(transform);
                        UnityEngine.Object.Destroy(transform);
#if ENABLE_LOG_PREFABPOOL
					if (logMessages)
					{
						//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): CULLING to {cullAbove} instances. Now at {totalCount}.");
					}
#endif
                    }
                    else if (logMessages)
                    {
#if ENABLE_LOG_PREFABPOOL
					//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): CULLING waiting for despawn. Checking again in {cullDelay}sec");
#endif
                        break;
                    }
                }
                //yield return new WaitForSeconds(cullDelay);
            }
#if ENABLE_LOG_PREFABPOOL
		if (logMessages)
		{
			//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): CULLING FINISHED! Stopping");
		}
#endif
            cullingActive = false;
            yield return null;
        }

        internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
        {
            //if (limitInstances && limitFIFO && _spawned.Count >= limitAmount)
            //{
                //Transform transform = _spawned.First();
#if ENABLE_LOG_PREFABPOOL
			if (logMessages)
			{
				//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): LIMIT REACHED! FIFO=True. Calling despawning for {transform}...");
			}
#endif
                //DespawnInstance(transform);
            //}
            Transform transform2;
            if (_despawned.Count == 0)
            {
                transform2 = SpawnNew(pos, rot);
#if ENABLE_LOG_PREFABPOOL
			//Debug.LogError("Spaw new object: " + transform2.name);
#endif
            }
            else
            {
                transform2 = _despawned.First();
                _despawned.Remove(transform2);
                _spawned.Add(transform2);
                if (transform2 == null)
                {
                    string message = "Make sure you didn't delete a despawned instance directly.";
                    throw new MissingReferenceException(message);
                }
#if ENABLE_LOG_PREFABPOOL
			if (logMessages)
			{
				//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): respawning '{transform2.name}'.");
			}
#endif
                transform2.position = pos;
                transform2.rotation = rot;
                PoolUtils.SetActive(transform2.gameObject, state: true);
            }
            return transform2;
        }

        public Transform SpawnNew()
        {
            return SpawnNew(Vector3.zero, Quaternion.identity);
        }

        public Transform SpawnNew(Vector3 pos, Quaternion rot)
        {
            //if (limitInstances && totalCount >= limitAmount)
            //{
#if ENABLE_LOG_PREFABPOOL
			if (logMessages)
			{
				//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): LIMIT REACHED! Not creating new instances! (Returning null)");
			}
#endif
            //return null;
            //}
            if (pos == Vector3.zero)
            {
                pos = spawnPool.group.position;
            }
            if (rot == Quaternion.identity)
            {
                rot = spawnPool.group.rotation;
            }
            Transform transform = UnityEngine.Object.Instantiate(prefab, pos, rot);
            nameInstance(transform);
            //if (!spawnPool.dontReparent)
            //{
                transform.parent = spawnPool.group;
            //}
            //if (spawnPool.matchPoolScale)
            //{
            //    transform.localScale = Vector3.one;
            //}
            //if (spawnPool.matchPoolLayer)
            //{
            //    SetRecursively(transform, spawnPool.gameObject.layer);
            //}
            _spawned.Add(transform);
#if ENABLE_LOG_PREFABPOOL
		if (logMessages)
		{
			//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): Spawned new instance '{transform.name}'.");
		}

		if (totalCount > preloadAmount)
		{
			//Debug.LogWarning($"SpawnPool {spawnPool.poolName} ({prefab.name}): Spawned new instance '{transform.name}' overpreload totalCount={totalCount}, limit={preloadAmount}.");
		}
#endif
            return transform;
        }

        private void SetRecursively(Transform xform, int layer)
        {
            xform.gameObject.layer = layer;
            IEnumerator enumerator = xform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform xform2 = (Transform)enumerator.Current;
                    SetRecursively(xform2, layer);
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
        }

        internal void AddUnpooled(Transform inst, bool despawn)
        {
            nameInstance(inst);
            if (despawn)
            {
                PoolUtils.SetActive(inst.gameObject, state: false);
                _despawned.Add(inst);
            }
            else
            {
                _spawned.Add(inst);
            }
        }

        internal void PreloadInstances()
        {
            if (preloaded)
            {
#if ENABLE_LOG_PREFABPOOL
			//Debug.Log($"SpawnPool {spawnPool.poolName} ({prefab.name}): Already preloaded! You cannot preload twice. If you are running this through code, make sure it isn't also defined in the Inspector.");
#endif
                return;
            }
            if (prefab == null)
            {
#if ENABLE_LOG_PREFABPOOL
			//Debug.LogError($"SpawnPool {spawnPool.poolName} ({prefab.name}): Prefab cannot be null.");
#endif
                return;
            }
            //if (limitInstances && preloadAmount > limitAmount)
            //{
#if ENABLE_LOG_PREFABPOOL
			//Debug.LogWarning($"SpawnPool {spawnPool.poolName} ({prefab.name}): You turned ON 'Limit Instances' and entered a 'Limit Amount' greater than the 'Preload Amount'! Setting preload amount to limit amount.");
#endif
                //preloadAmount = limitAmount;
            //}
#if ENABLE_LOG_PREFABPOOL
		if (cullDespawned && preloadAmount > cullAbove)
		{
			//Debug.LogWarning($"SpawnPool {spawnPool.poolName} ({prefab.name}): You turned ON Culling and entered a 'Cull Above' threshold greater than the 'Preload Amount'! This will cause the culling feature to trigger immediatly, which is wrong conceptually. Only use culling for extreme situations. See the docs.");
		}
#endif
            //if (preloadTime)
            //{
                //if (preloadFrames > preloadAmount)
                //{
#if ENABLE_LOG_PREFABPOOL
				//Debug.LogWarning($"SpawnPool {spawnPool.poolName} ({prefab.name}): Preloading over-time is on but the frame duration is greater than the number of instances to preload. The minimum spawned per frame is 1, so the maximum time is the same as the number of instances. Changing the preloadFrames value...");
#endif
                    //preloadFrames = preloadAmount;
                //}
                //spawnPool.StartCoroutine(PreloadOverTime());
                //return;
            //}
            //forceLoggingSilent = true;
            //while (totalCount < preloadAmount)
            //{
            //    Transform xform = SpawnNew();
            //    DespawnInstance(xform, sendEventMessage: false);
            //}
            //forceLoggingSilent = false;
        }

        //private IEnumerator PreloadOverTime()
        //{
        //    yield return new WaitForSeconds(preloadDelay);
        //    int amount = preloadAmount - totalCount;
        //    if (amount <= 0)
        //    {
        //        yield break;
        //    }
        //    int remainder = amount % preloadFrames;
        //    int numPerFrame = amount / preloadFrames;
        //    forceLoggingSilent = true;
        //    for (int j = 0; j < preloadFrames; j++)
        //    {
        //        int numThisFrame = numPerFrame;
        //        if (j == preloadFrames - 1)
        //        {
        //            numThisFrame += remainder;
        //        }
        //        for (int i = 0; i < numThisFrame; i++)
        //        {
        //            Transform inst = SpawnNew();
        //            if (inst != null)
        //            {
        //                DespawnInstance(inst, sendEventMessage: false);
        //            }
        //            yield return null;
        //        }
        //        if (totalCount > preloadAmount)
        //        {
        //            break;
        //        }
        //    }
        //    forceLoggingSilent = false;
        //}

        private void nameInstance(Transform instance)
        {
            instance.name += (totalCount + 1).ToString("#000");
        }
    }
}