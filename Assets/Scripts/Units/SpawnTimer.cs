using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTimer : Singleton<SpawnTimer>
{

    public List<Spawner> playerSpawner;

    public List<Spawner> enemySpawner;


    public float timer;
    public float spawnTime = 10f;

    private void Start()
    {
        timer = 0f;
    }


    private void Update()
    {
        if (timer < spawnTime)
        {
            timer += Time.deltaTime;
        }

        if (timer >= spawnTime)
        {
            for (int i = 0; i < playerSpawner.Count; i++)
            {
                BuildingManager.Instance.UpdateSpawnPool();
                StartCoroutine(playerSpawner[i].Spawn(BuildingManager.Instance.spawnPool));
            }
            for (int i = 0; i < enemySpawner.Count; i++)
            {
                if (enemySpawner[i] != null)
                    enemySpawner[i].Spawn();
            }

            timer = 0f;
        }
    }


}
