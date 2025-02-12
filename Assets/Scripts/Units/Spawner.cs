using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public Transform[] spawnPoints;
	public Transform[] rallyPoints;
    public int enemySpawnerIdx;
    public int team;

	public IEnumerator Spawn(SpawnTimer.EnemySpawnInfo spawnInfo)
	{
        foreach (var v in spawnInfo.spawns)
        {
            var obj = SpawnTimer.Instance.enemyPrefabs[v.Key];
            int spawnCycle = 0;
            for (int n = 0; n < v.Value; n++)
            {
                var go = Instantiate(obj, spawnPoints[spawnCycle].position, spawnPoints[spawnCycle].rotation);
                var spawned = go.GetComponent<RTSUnit>();
                SetSpawned(spawned, spawnCycle);
                spawnCycle++;
                if (spawnCycle >= 3)
                    spawnCycle = 0;
                yield return CoroutineHelper.WaitForSeconds(0.5f);
            }
        }
    }

	public IEnumerator Spawn(List<BuildingManager.SpawnInfo> pool, int posIdx)
    {
		foreach(var v in pool)
        {
            var go = Instantiate(v.obj, spawnPoints[posIdx].position, spawnPoints[posIdx].rotation);
            var spawned = go.GetComponent<RTSUnit>();
            SetSpawned(spawned, posIdx);

            yield return CoroutineHelper.WaitForSeconds(0.5f);
        }
    }

    private void SetSpawned(RTSUnit spawned, int posIdx)
    {
        spawned.team = team;
        if (team == 2)
        {
            spawned.sprite.flipX = true;
        }
        else
        {
            spawned.sprite.flipX = false;
        }
        spawned.SetDestination(rallyPoints[posIdx].position, MovementMode.AttackMove);
    }
}
