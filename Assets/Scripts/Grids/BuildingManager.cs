using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingManager : Singleton<BuildingManager>
{

    public List<Building> playerBuildings;


    public Dictionary<GameObject, int> spawnPool;


    public void UpdateSpawnPool()
    {
        spawnPool = new Dictionary<GameObject, int>();

        for (int i = 0; i < playerBuildings.Count; i++)
        {
            // 스폰 체크
            var su = playerBuildings[i].spawnUnit;
            int count = playerBuildings[i].spawnCount;
            if (su != null)
            {
                if (spawnPool.ContainsKey(su))
                {
                    spawnPool[su] += count;
                }
                else
                {
                    spawnPool.Add(su, count);
                }

            }
        }
    }

}
