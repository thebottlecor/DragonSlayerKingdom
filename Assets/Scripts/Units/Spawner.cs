using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public GameObject[] prefab;

	public Transform spawnPoint;
	public Transform rallyPoint;
    public int team;

	public void Spawn()
	{
		var go = Instantiate(prefab[0], spawnPoint.position, spawnPoint.rotation);
		var spawned = go.GetComponent<RTSUnit>();
        SetSpawned(spawned);
    }

	public IEnumerator Spawn(List<BuildingManager.SpawnInfo> pool)
    {
		foreach(var v in pool)
        {
            var go = Instantiate(v.obj, spawnPoint.position, spawnPoint.rotation);
            var spawned = go.GetComponent<RTSUnit>();
            SetSpawned(spawned);

            yield return CoroutineHelper.WaitForSeconds(0.5f);
        }
    }

    private void SetSpawned(RTSUnit spawned)
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
        spawned.SetDestination(rallyPoint.position, MovementMode.AttackMove);
    }
}
