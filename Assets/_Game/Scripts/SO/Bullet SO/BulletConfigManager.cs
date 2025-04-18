using UnityEngine;
using System.Collections.Generic;

public class BulletConfigManager : MonoBehaviour
{
    [SerializeField] private BulletConfigList bulletConfigList;
    private Dictionary<string, BulletData> bulletDataCache = new Dictionary<string, BulletData>();

    private void Awake()
    {
        // Tìm BulletConfigList nếu chưa được gán
        if (bulletConfigList == null)
        {
            bulletConfigList = GetComponent<BulletConfigList>();
            if (bulletConfigList == null)
            {
                Debug.LogError("BulletConfigList not found on BulletConfigManager!");
                return;
            }
        }

        // Khởi tạo và lưu trữ BulletData từ tất cả BulletConfig
        InitializeBulletData();
    }

    private void InitializeBulletData()
    {
        // Giả sử BulletConfigList có một danh sách các BulletConfig
        foreach (BulletConfig config in bulletConfigList.GetAllConfigs())
        {
            if (config == null)
            {
                Debug.LogWarning("Found null BulletConfig in BulletConfigList!");
                continue;
            }

            string configName = config.name; // Tên của BulletConfig (hoặc dùng bulletType như bullet_1, bullet_2)
            if (!bulletDataCache.ContainsKey(configName))
            {
                BulletData data = new BulletData(config);
                bulletDataCache.Add(configName, data);
                Debug.Log($"BulletConfigManager: Cached BulletData for {configName} with damage: {data.damage}");
            }
        }
    }

    // Lấy BulletData theo tên (bulletType)
    public BulletData GetBulletData(string bulletType)
    {
        if (bulletDataCache.ContainsKey(bulletType))
        {
            return bulletDataCache[bulletType];
        }

        Debug.LogWarning($"BulletData for {bulletType} not found in cache!");
        return null;
    }

    // Lấy tất cả BulletData (nếu cần)
    public Dictionary<string, BulletData> GetAllBulletData()
    {
        return bulletDataCache;
    }
}