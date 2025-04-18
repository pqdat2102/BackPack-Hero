using UnityEngine;
using UnityEngine.UI; // Nếu dùng Text UI
using TMPro; // Nếu dùng TextMeshPro

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance { get; private set; }

    [Header("Resources")]
    [SerializeField] private int gold = 0;
    [SerializeField] private int experience = 0;
    [SerializeField] private int levelBuff = 1;
    [SerializeField] private int expPerLevel = 50;

    [Header("Upgrade")]
    [SerializeField] private UpdateUI updateUI; 
    [SerializeField] private UpGrageBuffUI updateBuff;
    [SerializeField] private Dice dice;

    private bool isWaitingForUpgrade = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        updateUI.UpdateUIinGameplayScene();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        updateUI.UpdateUIinGameplayScene();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        CheckLevelUp();
        updateUI.UpdateUIinGameplayScene();
    }

    private void CheckLevelUp()
    {
        while (experience >= expPerLevel)
        {
            experience -= expPerLevel;
            levelBuff++;

            // Hiển thị popup nâng cấp
            ShowUpgradePopup();
        }

        if (levelBuff >= 4) expPerLevel = 100;
        if (levelBuff >= 8) expPerLevel = 200;
    }

    private void ShowUpgradePopup()
    {
        if (updateBuff != null && dice != null)
        {
            isWaitingForUpgrade = true;
            Time.timeScale = 0f; // Dừng thời gian game

            updateBuff.ShowPopup(dice, () =>
            {
                if (!isWaitingForUpgrade)
                {
                    Time.timeScale = 1f;
                }
            });
        }
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetLevelBuff()
    {
        return levelBuff;
    }

    public int GetExpPerLevel()
    {
        return expPerLevel;
    }   
    public bool IsWaitingForUpgrade()
    {
        return isWaitingForUpgrade;
    }

    public void SetWaitingForUpgrade(bool value)
    {
        isWaitingForUpgrade = value;
    }
}