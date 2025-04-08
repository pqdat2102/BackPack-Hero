using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByDie : Despawn
{

    [SerializeField] private bool isDead;

    private void SetDead()
    {
        this.isDead = false;
    }

    protected override bool CanDespawn()
    {
        return isDead;
    }
}
