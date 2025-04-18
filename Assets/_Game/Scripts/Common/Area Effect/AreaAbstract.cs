using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAbstract : DicevsMonsterMonobehavior
{
    [Header("Area Abstract")]
    [SerializeField] protected AreaController areaController;
    public AreaController AreaController { get => areaController; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletController();
    }

    protected virtual void LoadBulletController()
    {
        if (this.areaController != null) return;
        this.areaController = transform.parent.GetComponent<AreaController>();
        Debug.Log(transform.name + ": LoadBulletController", gameObject);
    }
}
