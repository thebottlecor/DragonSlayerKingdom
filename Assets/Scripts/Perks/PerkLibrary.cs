using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ResearchLibrary", menuName = "Library/Research")]
public class PerkLibrary : ScriptableObject
{
    [SerializeField]
    private List<PerkInfo> infos = null;

    public Dictionary<int, PerkInfo> GetHashMap()
    {
        Dictionary<int, PerkInfo> hashMap = new Dictionary<int, PerkInfo>();
        foreach (var v in infos)
        {
            if (hashMap.ContainsKey(v.idx))
                continue;

            hashMap.Add(v.idx, v);
        }
        return hashMap;
    }
}
