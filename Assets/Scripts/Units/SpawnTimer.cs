using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine.UI;

public class SpawnTimer : Singleton<SpawnTimer>
{

    public List<Spawner> playerSpawner;

    public List<Spawner> enemySpawner;

    public GameObject[] enemyPrefabs;

    [SerializedDictionary("업데이트 웨이브", "스폰정보")]
    public SerializedDictionary<int, EnemySpawnInfo> enemySpawnInfos;
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        [SerializedDictionary("프리팹 인덱스", "스폰수")]
        public SerializedDictionary<int, int> spawns;
    }

    public EnemySpawnInfo currentSpawnInfos;

    [SerializedDictionary("업데이트 웨이브", "보너스골드")]
    public SerializedDictionary<int, int> bonusGolds;

    [Space(30f)]
    public float timer;
    public float spawnTime = 10f;
    public int cycle;
    public Slider timerSlider;

    public float gameTimer;
    public float dragonTime = 1200f;

    public TextMeshProUGUI gameTimerText;

    private void Start()
    {
        timer = 0f;
        gameTimer = 0f;
    }


    private void Update()
    {
        gameTimer += Time.deltaTime;

        float remainTimer = Mathf.Max(0f, dragonTime - gameTimer);
        int hour = (int)(remainTimer / 3600f);
        int minute = (int)((remainTimer - hour * 3600f) / 60f);
        int sec = (int)(remainTimer - minute * 60f - hour * 3600f);

        if (hour <= 0)
        {
            gameTimerText.text = $"{minute:00}:{sec:00}";
        }
        else
        {
            gameTimerText.text = $"{hour:00}:{minute:00}:{sec:00}";
        }

        if (timer < spawnTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            BuildingManager.Instance.UpdateSpawnPool();
            if (playerSpawner[0] != null)
            {
                StartCoroutine(playerSpawner[0].Spawn(BuildingManager.Instance.spawnPool, 0));
                StartCoroutine(playerSpawner[0].Spawn(BuildingManager.Instance.spawnPool_up, 1));
                StartCoroutine(playerSpawner[0].Spawn(BuildingManager.Instance.spawnPool_down, 2));
            }
            //StartCoroutine(playerSpawner[1].Spawn(BuildingManager.Instance.spawnPool_up));
            //StartCoroutine(playerSpawner[2].Spawn(BuildingManager.Instance.spawnPool_down));

            if (enemySpawnInfos.ContainsKey(cycle))
                currentSpawnInfos = enemySpawnInfos[cycle];

            if (bonusGolds.ContainsKey(cycle))
                BuildingManager.Instance.bonus_gold = bonusGolds[cycle];

            for (int i = 0; i < enemySpawner.Count; i++)
            {
                if (enemySpawner[i] != null)
                {
                    StartCoroutine(enemySpawner[i].Spawn(currentSpawnInfos));
                    break; // 최전방에서만 스폰
                }
            }
            BuildingManager.Instance.YieldState(true);
            timer = 0f;

            cycle++;
        }

        float cyclePercent = timer / spawnTime;
        timerSlider.value = cyclePercent;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.timeScale >= 1f)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.timeScale < 5f)
                Time.timeScale = 5f;
            else
                Time.timeScale = 1f;
        }
    }


}
