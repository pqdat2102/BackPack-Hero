using UnityEngine;

public class AreaDespawn : DespawnByTime    
{
    private float customLifetime;

    public void SetLifetime(float lifetime)
    {
        this.customLifetime = lifetime;
        this.timeLimit = lifetime;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        if (customLifetime > 0)
        {
            this.timeLimit = customLifetime;
        }
    }

    public override void DespawnObject()
    {
        AreaSpawner.Instance.Despawn(transform.parent);
    }
}