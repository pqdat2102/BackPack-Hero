using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPool
{
    public static class PoolUtils
    {
        internal static void SetActive(GameObject obj, bool state)
        {
            obj.SetActive(state);
        }

        internal static bool activeInHierarchy(GameObject obj)
        {
            return obj.activeInHierarchy;
        }
    }
}
