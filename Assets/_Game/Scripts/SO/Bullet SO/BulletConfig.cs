using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Data/Bullet/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    public enum MovementType
    {
        Straight, Curved
    }

    //[Header("General")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public int damage = 1;
    [SerializeField] public MovementType movementType = MovementType.Straight;

    //[Header("Curved Movement Parameters")]
    [SerializeField] public float rotationSpeed = 10f;
    [SerializeField] public float curvedHeight = 2f;

    //[Header("Effect")]
    [SerializeField] public BulletEffectType effectType = BulletEffectType.None;
    [SerializeField] public float effectDelay = 0f;

    //[Header("Explosion Effect Parameters")]
    [SerializeField] public BulletConfig[] childBulletConfig;
    [SerializeField] public int childBulletCount = 5;
    [SerializeField] public float explosionRadius = 2f;

    /*[Header("Area Effect Parameters")]*/
    [SerializeField] public GameObject areaEffectPrefab;
    [SerializeField] public float areaLifetime = 2f;
    [SerializeField] public float areaDamage = 1f;
    [SerializeField] public float areaTimeGetDamage = 0.1f; // thời gian giữa 2 lần kích hoạt collider
}