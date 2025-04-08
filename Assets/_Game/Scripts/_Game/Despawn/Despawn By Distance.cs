using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByDistance : Despawn
{
    [SerializeField] protected float disLimit = 15f;
    [SerializeField] protected float distance = 0f;
    [SerializeField] protected Camera mainCamera;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCamera();
    }

    protected virtual void LoadCamera()
    {
        if (this.mainCamera != null) return;
        this.mainCamera = Transform.FindObjectOfType<Camera>();
    }

    protected override bool CanDespawn()
    {
        this.distance = Vector3.Distance(transform.position, this.mainCamera.transform.position);
        if (this.distance > this.disLimit) return true;
        return false;
    }
}
