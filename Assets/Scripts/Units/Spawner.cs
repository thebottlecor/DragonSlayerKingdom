using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples.RTS;

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

		spawned.team = unit.team;
		spawned.SetDestination(rallyPoint.position, MovementMode.AttackMove);
	}

	public IEnumerator Spawn(Dictionary<GameObject, int> pool)
    {
		foreach(var v in pool)
        {
			for (int i = 0; i < v.Value; i++)
			{
				var go = GameObject.Instantiate(v.Key, spawnPoint.position, spawnPoint.rotation);
				var spawned = go.GetComponent<RTSUnit>();

				spawned.team = unit.team;
				spawned.SetDestination(rallyPoint.position, MovementMode.AttackMove);

				yield return null;
			}
		}
    }
}
