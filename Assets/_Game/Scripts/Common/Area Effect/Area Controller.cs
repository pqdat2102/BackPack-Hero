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
        if (activeZones.TryGetValue(enemy, out var existingImpact) && existingImpact != null)
        {
            // hard code tại chưa làm SO
            existingImpact.ScaleSize(1.2f, 1.4f);
            return;
        }

        Transform zone = AreaSpawner.Instance.Spawn(AreaSpawner.area_effect, position, rotation);
        if (zone == null) return;

        var zoneController = zone.GetComponent<AreaController>();
        if (zoneController == null) return;

        zone.gameObject.SetActive(true);

        // Khởi tạo các thành phần của zone mới spawn
        zoneController.areaImpact.Initialize(data.areaDamage, data.areaLifetime, data.areaTimeGetDamage, enemy);
        activeZones[enemy] = zoneController.areaImpact;

        // Bắt đầu coroutine trên zone mới
        zoneController.StartCoroutine(zoneController.WaitDespawnArea(data.areaLifetime));
    }


    private IEnumerator WaitDespawnArea(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (areaDespawn != null)
        {
            areaDespawn.DespawnObject();
        }
    }
    public void RemoveZone(Transform enemy)
    {
        if (activeZones.ContainsKey(enemy))
        {
            activeZones.Remove(enemy);
        }
    }
}