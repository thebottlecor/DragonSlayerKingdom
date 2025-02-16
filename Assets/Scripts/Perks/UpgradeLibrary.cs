using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UpgradeLibrary", menuName = "Library/Upgrade")]
public class UpgradeLibrary : ScriptableObject
{
    [SerializeField]
    private List<UpgradeInfo> infos = null;

    public Dictionary<int, UpgradeInfo> GetHashMap()
    {
        Dictionary<int, UpgradeInfo> hashMap = new Dictionary<int, UpgradeInfo>();
        foreach (var v in infos)
        {
            if (hashMap.ContainsKey(v.idx))
                continue;

            hashMap.Add(v.idx, v);
        }
        return hashMap;
    }
}
