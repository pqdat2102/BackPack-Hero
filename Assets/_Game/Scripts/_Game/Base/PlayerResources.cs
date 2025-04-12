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

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelBuffText;

    [Header("Upgrade Popup")]
    [SerializeField] private UpdateBuff updateBuff; // Tham chiếu đến UpgradePopup
    [SerializeField] private Dice dice; // Tham chiếu đến Dice

    private bool isWaitingForUpgrade = false; // Trạng thái chờ người chơi chọn nâng cấp

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Người chơi nhận được {amount} vàng. Tổng vàng: {gold}");
        UpdateUI();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Người chơi nhận được {amount} kinh nghiệm. Tổng kinh nghiệm: {experience}");

        CheckLevelUp();
        UpdateUI();
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
                isWaitingForUpgrade = false;
                Time.timeScale = 1f; 
            });
        }
        else
        {
            Debug.LogWarning("UpgradePopup hoặc Dice chưa được gán trong PlayerResources!");
        }
    }

    private void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = $"GOLD: {gold}";
        }
        if (expText != null)
        {
            expText.text = $"EXP: {experience}/{expPerLevel}";
        }
        if (levelBuffText != null)
        {
            levelBuffText.text = $"LEVEL BUFF: {levelBuff}";
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

    // (Tùy chọn) Kiểm tra xem có đang chờ người chơi chọn nâng cấp không
    public bool IsWaitingForUpgrade()
    {
        return isWaitingForUpgrade;
    }
}