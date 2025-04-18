using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespaw : DespawnByDistance
{
    public override void DespawnObject()
    {
        BulletSpawner.Instance.Despawn(transform.parent);
    }
}
