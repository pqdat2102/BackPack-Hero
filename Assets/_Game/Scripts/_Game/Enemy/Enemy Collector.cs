/*using UnityEngine;
using System.Collections.Generic;

public class EnemyCollector : MonoBehaviour
{
    [SerializeField] private Transform enemyHolder;

    protected void Awake()
    {
        if (enemyHolder == null)
        {
            enemyHolder = FindObjectOfType<EnemyHolder>()?.transform;
            if (enemyHolder == null)
            {
                Debug.LogWarning($"{name}: EnemyHolder not found!", gameObject);
            }
        }
    }

    public List<Transform> CollectEnemies()
    {
        List<Transform> enemies = new List<Transform>();
        if (enemyHolder == null) return enemies;

        foreach (Transform child in enemyHolder)
        {
            if (child.gameObject.activeInHierarchy)
            {
                enemies.Add(child);
            }
        }

        return enemies;
    }
}*/