using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BulletController : DicevsMonsterMonobehavior
{
    [SerializeField] private Transform model;
    public Transform Model { get => model; }

    [SerializeField] private BulletDamageSender bulletDamageSender;
    public BulletDamageSender BulletDamageSender { get => bulletDamageSender; }

    [SerializeField] private BulletDespaw bulletDespaw;
    public BulletDespaw BulletDespaw { get => bulletDespaw; }

    [SerializeField] private BulletData bulletData;
    public BulletData BulletData { get => bulletData; }

    [SerializeField] private AreaController areaController;
    public AreaController AreaController { get => areaController; }

    private IMove moveStrategy;
    private Transform target;


    protected override void OnEnable()
    {
        base.OnEnable();
        if (bulletData != null)
        {
            Initialize();
        }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadBulletDamageSender();
        this.LoadBulletDespawn();
    }

    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
        Debug.Log(transform.name + ": LoadModel", gameObject);
    }

    protected virtual void LoadBulletDamageSender()
    {
        if (bulletDamageSender != null) return;
        bulletDamageSender = GetComponentInChildren<BulletDamageSender>();
        Debug.Log(transform.name + ": Load BulletDamageSender", gameObject);
    }

    protected virtual void LoadBulletDespawn()
    {
        if (bulletDespaw != null) return;
        bulletDespaw = GetComponentInChildren<BulletDespaw>();
        Debug.Log(transform.name + ": Load BulletDespawn", gameObject);
    }

    protected virtual void LoadAreaController()
    {
        if (areaController != null) return;
        areaController = FindObjectOfType<AreaController>();
        if (areaController == null)
        {
            Debug.LogWarning("AreaController not found in scene!");
        }
    }

    public void SetData(BulletData newData)
    {
        bulletData = newData;
        Initialize();
    }

    public BulletData GetData()
    {
        return bulletData;
    }

    protected override void Reset()
    {
        base.Reset();
        UpdateConfigFromPrefab();
    }

    private void Initialize()
    {
        if (bulletData == null)
        {
            Debug.LogError($"{gameObject.name}: BulletConfig is null!");
            if (bulletDespaw != null) bulletDespaw.DespawnObject();
            return;
        }

        if (bulletDamageSender != null)
        {
            bulletDamageSender.SetDamage(bulletData.damage);
        }
    }
    private void UpdateConfigFromPrefab()
    {
#if UNITY_EDITOR
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();

        BulletSpawner spawner = FindObjectOfType<BulletSpawner>();
        if (spawner == null)
        {
            Debug.LogWarning($"{name}: Could not find BulletSpawner!");
            return;
        }

        Transform prefabsFolder = spawner.transform.Find("Prefabs");
        if (prefabsFolder == null)
        {
            Debug.LogWarning($"{name}: Could not find Prefabs folder in BulletSpawner!");
            return;
        }

        bool prefabExists = false;
        foreach (Transform prefab in prefabsFolder)
        {
            if (prefab.name == prefabName)
            {
                prefabExists = true;
                break;
            }
        }

        if (!prefabExists)
        {
            Debug.LogWarning($"{name}: Could not find prefab named {prefabName} in BulletSpawner!");
            return;
        }

        BulletShooter shooter = FindObjectOfType<BulletShooter>();
        if (shooter == null)
        {
            Debug.LogWarning($"{name}: Could not find BulletShooter!");
            return;
        }

        BulletConfigList configList = shooter.GetComponent<BulletConfigList>();
        if (configList == null)
        {
            Debug.LogWarning($"{name}: Could not find BulletConfigList!");
            return;
        }
#endif
    }

   
}