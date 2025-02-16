using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitLibrary", menuName = "Library/Unit")]
public class UnitLibrary : ScriptableObject
{
    [SerializeField]
    private List<UnitInfo> infos = null;

    public Dictionary<UnitIdx, UnitInfo> GetHashMap()
    {
        Dictionary<UnitIdx, UnitInfo> hashMap = new Dictionary<UnitIdx, UnitInfo>();
        foreach (var v in infos)
        {
            if (hashMap.ContainsKey(v.idx))
                continue;

            hashMap.Add(v.idx, v);
        }
        return hashMap;
    }
}
