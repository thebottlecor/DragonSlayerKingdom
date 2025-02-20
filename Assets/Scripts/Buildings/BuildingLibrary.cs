using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingLibrary", menuName = "Library/Building")]
public class BuildingLibrary : ScriptableObject
{
    [SerializeField]
    private List<BuildingInfo> infos = null;

    public Dictionary<int, BuildingInfo> GetHashMap()
    {
        Dictionary<int, BuildingInfo> hashMap = new Dictionary<int, BuildingInfo>();
        foreach (var v in infos)
        {
            if (hashMap.ContainsKey(v.idx))
                continue;

            hashMap.Add(v.idx, v);
        }
        return hashMap;
    }
}
