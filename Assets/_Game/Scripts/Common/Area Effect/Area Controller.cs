using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.FullSerializer;

public class AreaController : DicevsMonsterMonobehavior
{
    [SerializeField] protected AreaDamageSender areaDamageSender;
    public AreaDamageSender AreaDamageSender { get => areaDamageSender; }

    [SerializeField] protected AreaDespawn areaDespawn;
    public AreaDespawn AreaDespawn { get => areaDespawn; }

    [SerializeField] protected AreaImpact areaImpact;
    public AreaImpact AreaImpact { get => areaImpact; } 

    private static Dictionary<Transform, AreaImpact> activeZones = new Dictionary<Transform, AreaImpact>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAreaDamageSender();
        this.LoadAreaDespawn();
        this.LoadAreaImpact();
    }

    protected virtual void LoadAreaDamageSender()
    {
        if (this.areaDamageSender != null) return;
        this.areaDamageSender = GetComponentInChildren<AreaDamageSender>();
        Debug.Log(transform.name + ": LoadAreaDamageSender", gameObject);
    }

    protected virtual void LoadAreaDespawn()
    {
        if (this.areaDespawn != null) return;
        this.areaDespawn = GetComponentInChildren<AreaDespawn>();
        Debug.Log(transform.name + ": LoadAreaDespawn", gameObject);
    }

    protected virtual void LoadAreaImpact()
    {
        if (this.areaImpact != null) return;
        this.areaImpact = GetComponentInChildren<AreaImpact>();
        Debug.Log(transform.name + ": LoadAreaImpact", gameObject);
    }

    public void ActivateAreaEffect(Vector3 position, Quaternion rotation, BulletData data, Transform enemy)
    {
        if (activeZones.ContainsKey(enemy) && activeZones[enemy] != null)
        {
            activeZones[enemy].ScaleSize(1.2f, 1.4f);
        }
        else
        {
            Transform zone = AreaSpawner.Instance.Spawn(AreaSpawner.area_effect, position, rotation);
            if (zone == null)
            {
                Debug.LogWarning("Failed to spawn Area Effect from AreaSpawner!");
                return;
            }

            if (areaImpact != null)
            {
                areaImpact.Initialize(data.areaDamage, data.areaLifetime, enemy);
                activeZones[enemy] = areaImpact;
            }

            zone.gameObject.SetActive(true);

            var zoneController = zone.GetComponent<AreaController>();
            if (zoneController != null)
            {
                zoneController.StartCoroutine(zoneController.WaitDespawnArea());
            }
        }
    }

    private IEnumerator WaitDespawnArea()
    {
        yield return new WaitForSeconds(3);
        areaDamageSender.DespawnArea();
    }

    public void RemoveZone(Transform enemy)
    {
        if (activeZones.ContainsKey(enemy))
        {
            activeZones.Remove(enemy);
        }
    }
}