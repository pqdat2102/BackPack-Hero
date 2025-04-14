using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelBuffText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Player Resources")]
    [SerializeField] private PlayerResources playerResources;

    [Header("Enemy Spawn Wave")]
    [SerializeField] protected EnemySpawnWave enemySpawnWave;


    public void UpdateUIinGameplayScene()
    {
        if (goldText != null)
        {
            goldText.text = $"GOLD: {playerResources.GetGold()}";
        }
        if (expText != null)
        {
            expText.text = $"EXP: {playerResources.GetExperience()}/{playerResources.GetExpPerLevel()}";
        }
        if (levelBuffText != null)
        {
            levelBuffText.text = $"LEVEL BUFF: {playerResources.GetLevelBuff()}";
        }
    }

    public virtual void UpdateWaveText()
    {
        waveText.text = "Wave: " + (enemySpawnWave.GetCurrentWave() + 1) + "/" + enemySpawnWave.GetTotalWaves();
    }

}
