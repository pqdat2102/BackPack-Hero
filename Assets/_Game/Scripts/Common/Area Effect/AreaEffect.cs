// AreaEffect.cs
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class AreaEffect : IBulletEffect
{
    private AreaController areaController;

    public AreaEffect(AreaController areaController)
    {
        this.areaController = areaController;
    }
    public void ApplyEffect(Transform target, BulletData data)
    {
        if (data.effectType == BulletEffectType.Area)
        {
            areaController.ActivateAreaEffect(target.position, Quaternion.identity, data, target);
        }
    }
}