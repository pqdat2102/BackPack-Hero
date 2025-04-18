using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BulletConfigList", menuName = "Data/Bullet/BulletConfigList")]
public class BulletConfigList : ScriptableObject
{
    [SerializeField] private List<BulletConfigEntry> bulletConfigs = new List<BulletConfigEntry>();

    public BulletConfig GetConfig(int index)
    {
        if (index >= 0 && index < bulletConfigs.Count)
        {
            return bulletConfigs[index].config;
        }
        Debug.LogWarning($"BulletConfig index {index} out of range");
        return null;
    }

    public BulletConfig GetConfigByName(string name)
    {
        BulletConfigEntry entry = bulletConfigs.Find(e => e.name == name);
        if (entry != null && entry.config != null)
        {
            return entry.config;
        }
        Debug.LogWarning($"BulletConfig with name {name} not found");
        return null;
    }

    public int GetConfigCount()
    {
        return bulletConfigs.Count;
    }

    // Thêm phương thức để lấy tất cả BulletConfig
    public List<BulletConfig> GetAllConfigs()
    {
        List<BulletConfig> configs = new List<BulletConfig>();
        foreach (BulletConfigEntry entry in bulletConfigs)
        {
            if (entry.config != null)
            {
                configs.Add(entry.config);
            }
        }
        return configs;
    }
}

[System.Serializable]
public class BulletConfigEntry
{
    public string name;
    public BulletConfig config;
}