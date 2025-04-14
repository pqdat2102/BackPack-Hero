using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EnemySpawnWave : DicevsMonsterMonobehavior
{
    [Header("Enemy Spawner Wave")]
    [SerializeField] protected EnemySpawnerController enemySpawnerController;
    [SerializeField] protected UpdateUI updateUI;
    [SerializeField] protected List<WaveConfig> waves = new List<WaveConfig>(); // Danh sách các wave
    [SerializeField] protected float spawnRadius = 5f; // Bán kính vùng spawn ngẫu nhiên

    private int currentWave = 0;
    private bool isSpawning = false;
    private List<WaveEnemy> currentWaveEnemies = new List<WaveEnemy>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyController();
    }
    protected virtual void LoadEnemyController()
    {
        if (this.enemySpawnerController != null) return;
        this.enemySpawnerController = GetComponent<EnemySpawnerController>();
        Debug.Log(transform.name + ": Load EnemyController", gameObject);
    }

    protected override void Start()
    {
        if (waves.Count == 0)
        {
            Debug.LogWarning("No waves configured! Adding default wave.");
            AddDefaultWave();
        }
        updateUI.UpdateWaveText();
        // Bắt đầu wave đầu tiên ngay lập tức
        StartFirstWave();
    }

    protected virtual void StartFirstWave()
    {
        isSpawning = true;
        PrepareWaveEnemies();
    }

    protected virtual void FixedUpdate()
    {
        if (!isSpawning) return;
        this.EnemySpawning();
    }

    protected virtual void EnemySpawning()
    {
        if (currentWaveEnemies.Count <= 0)
        {

            EndWave();
            return;
        }

        //Debug.Log(currentWaveEnemies.Count + " enemies remaining in wave " + currentWave);

        // Lấy điểm spawn chính
        Transform spawnPoint = this.enemySpawnerController.EnemySpawnPoints.GetRandom();
        Vector3 centerPos = spawnPoint.position;

        // Spawn tất cả quái trong wave cùng một lúc với vị trí ngẫu nhiên
        foreach (WaveEnemy enemy in currentWaveEnemies)
        {
            // Tạo vị trí ngẫu nhiên trong vùng spawn
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPos = centerPos + new Vector3(randomCircle.x, randomCircle.y, 0);
            Quaternion rot = transform.rotation;

            Transform obj = this.enemySpawnerController.EnemySpawner.Spawn(enemy.enemyPrefab, randomPos, rot);
            obj.gameObject.SetActive(true);
        }
        currentWaveEnemies.Clear();
    }

    protected virtual void EndWave()
    {
        isSpawning = false;
        currentWave++;

        if (currentWave < waves.Count)
        {
            StartCoroutine(StartNextWave());
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    protected virtual IEnumerator StartNextWave()
    {
        // Lấy thời gian delay từ wave hiện tại
        float delay = waves[currentWave].delayBeforeWave;
        
        // Nếu là wave đầu tiên không có delay
        if (currentWave == 0 /*|| currentWave == waves.Count - 1*/)
        {
            delay = 0f;
        }
        
        yield return new WaitForSeconds(delay);
        updateUI.UpdateWaveText();
        isSpawning = true;
        PrepareWaveEnemies();
    }
    
    protected virtual void PrepareWaveEnemies()
    {
        // Chuẩn bị danh sách quái cho wave hiện tại
        WaveConfig currentWaveConfig = waves[currentWave];
        currentWaveEnemies = new List<WaveEnemy>();
        foreach (WaveEnemy enemy in currentWaveConfig.enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                currentWaveEnemies.Add(enemy);
            }
        }
        // Xáo trộn danh sách quái
        ShuffleList(currentWaveEnemies);
    }

    protected virtual void AddDefaultWave()
    {
        WaveConfig defaultWave = new WaveConfig();
        defaultWave.enemies = new List<WaveEnemy>
        {
            new WaveEnemy { enemyPrefab = this.enemySpawnerController.EnemySpawner.RandomPrefab(), count = 10 }
        };
        defaultWave.delayBeforeWave = 5f; // Delay mặc định 5 giây
        waves.Add(defaultWave);
    }

    protected virtual void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public virtual int GetCurrentWave()
    {
        return currentWave;
    }

    public virtual int GetTotalWaves()
    {
        return waves.Count;
    }
}

[System.Serializable]
public struct WaveEnemy
{
    public Transform enemyPrefab; // Prefab quái
    public int count; // Số lượng quái trong wave
}

[System.Serializable]
public struct WaveConfig
{
    public List<WaveEnemy> enemies; // Danh sách quái trong wave với số lượng cố định
    public float delayBeforeWave; // Thời gian delay trước khi bắt đầu wave này
}
