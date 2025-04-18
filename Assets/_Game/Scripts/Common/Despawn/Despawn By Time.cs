using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByTime : Despawn
{
    protected float timeLimit = 0.5f;
    protected override bool CanDespawn()
    {
       return timeLimit <= 0;
    }
}
