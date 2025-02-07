using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public GameObject[] prefab;

	public Transform spawnPoint;
	public Transform rallyPoint;
	RTSUnit unit;

	void Awake()
	{
		unit = GetComponent<RTSUnit>();
	}

	public void Spawn()
	{
		var go = GameObject.Instantiate(prefab[0], spawnPoint.position, spawnPoint.rotation);
		var spawned = go.GetComponent<RTSUnit>();
        SetSpawned(spawned);
    }

	public IEnumerator Spawn(Dictionary<GameObject, int> pool)
    {
		foreach(var v in pool)
        {
			for (int i = 0; i < v.Value; i++)
            {
                var go = GameObject.Instantiate(v.Key, spawnPoint.position, spawnPoint.rotation);
                var spawned = go.GetComponent<RTSUnit>();
                SetSpawned(spawned);

                yield return null;
                yield return null;
            }
        }
    }

    private void SetSpawned(RTSUnit spawned)
    {
        int team = unit.team;
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
