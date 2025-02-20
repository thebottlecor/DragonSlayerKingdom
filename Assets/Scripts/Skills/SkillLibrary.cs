using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillLibrary", menuName = "Library/Skill")]
public class SkillLibrary : ScriptableObject
{
    [SerializeField]
    private List<SkillInfo> infos = null;

    public Dictionary<Skill, SkillInfo> GetHashMap()
    {
        Dictionary<Skill, SkillInfo> hashMap = new Dictionary<Skill, SkillInfo>();
        foreach (var v in infos)
        {
            if (hashMap.ContainsKey(v.idx))
                continue;

            hashMap.Add(v.idx, v);
        }
        return hashMap;
    }
}
