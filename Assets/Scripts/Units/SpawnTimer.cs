using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpawnTimer : Singleton<SpawnTimer>
{

    public List<Spawner> playerSpawner;

    public List<Spawner> enemySpawner;


    public float timer;
    public float spawnTime = 10f;

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

        if (timer >= spawnTime)
        {
            BuildingManager.Instance.UpdateSpawnPool();
            StartCoroutine(playerSpawner[0].Spawn(BuildingManager.Instance.spawnPool));
            StartCoroutine(playerSpawner[1].Spawn(BuildingManager.Instance.spawnPool_up));
            StartCoroutine(playerSpawner[2].Spawn(BuildingManager.Instance.spawnPool_down));


            for (int i = 0; i < enemySpawner.Count; i++)
            {
                if (enemySpawner[i] != null)
                    enemySpawner[i].Spawn();
            }

            BuildingManager.Instance.YieldState(true);

            timer = 0f;
        }
    }


}
