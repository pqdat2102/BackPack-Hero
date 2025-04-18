using UnityEngine;

[System.Serializable]
public class BulletData
{
    public float speed;
    public int damage;
    public BulletConfig.MovementType movementType;
    public float rotationSpeed;
    public float curvedHeight;
    public BulletEffectType effectType;
    public float effectDelay;
    public BulletConfig[] childBulletConfig;
    public int childBulletCount;
    public float explosionRadius;
    public GameObject areaEffectPrefab;
    public float areaLifetime;
    public float areaDamage;
    public float areaTimeGetDamage;

    public BulletData(BulletConfig config)
    {
        this.speed = config.speed;
        this.damage = config.damage;
        this.movementType = config.movementType;
        this.rotationSpeed = config.rotationSpeed;
        this.curvedHeight = config.curvedHeight;
        this.effectType = config.effectType;
        this.effectDelay = config.effectDelay;
        this.childBulletConfig = config.childBulletConfig;
        this.childBulletCount = config.childBulletCount;
        this.explosionRadius = config.explosionRadius;
        this.areaEffectPrefab = config.areaEffectPrefab;
        this.areaLifetime = config.areaLifetime;
        this.areaDamage = config.areaDamage;
        this.areaTimeGetDamage = config.areaTimeGetDamage;
    }
}

public enum BulletEffectType
{
    None,
    Explosion,
    Area
}