using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerkManager : Singleton<PerkManager>
{

    public PerkSelectUI[] perkSelectUIs;

    public Dictionary<int, PerkInfo> Perks => DataManager.Instance.perks;

    public HashSet<int> activePerkIdxs;

    public Dictionary<UnitIdx, HashSet<Skill>> unitHasSkills;

    protected override void Awake()
    {
        base.Awake();

        activePerkIdxs = new HashSet<int>();

        unitHasSkills = new Dictionary<UnitIdx, HashSet<Skill>>();
        if (DataManager.Instance != null)
        {
            foreach (var v in DataManager.Instance.units)
            {
                var hasSkills = new HashSet<Skill>();
                if (v.Value.initSkills != null)
                {
                    for (int i = 0; i < v.Value.initSkills.Count; i++)
                    {
                        hasSkills.Add(v.Value.initSkills[i]);
                    }
                }
                unitHasSkills.Add(v.Key, hasSkills);
            }
        }
    }

    public void AddPerk(int idx)
    {
        if (!activePerkIdxs.Contains(idx))
            activePerkIdxs.Add(idx);

        CalcPerkEffect();
    }
    public void RemovePerk(int idx)
    {
        if (activePerkIdxs.Contains(idx))
            activePerkIdxs.Remove(idx);

        CalcPerkEffect();
    }

    public void CalcPerkEffect()
    {
        foreach (var v in activePerkIdxs)
        {
            PerkInfo perk = Perks[v];
            for (int n = 0; n < perk.targetUnitIdx.Count; n++)
            {
                UnitIdx idx = perk.targetUnitIdx[n];

                // 스킬 적용
                var newSkills = perk.addSkills;
                for (int i = 0; i < newSkills.Count; i++)
                {
                    if (!unitHasSkills[idx].Contains(newSkills[i]))
                        unitHasSkills[idx].Add(newSkills[i]);
                }
                // ??
            }
        }
    }

    public bool OpenPerkUI()
    {
        var available = GetAvailablePerks();

        int showingUICount = Mathf.Min(available.Count, perkSelectUIs.Length);

        if (showingUICount == 0)
        {
            Debug.Log("Nothing!");
            return false;
        }

        available.Shuffle();

        for (int i = 0; i < showingUICount; i++)
        {
            perkSelectUIs[i].gameObject.SetActive(true);
            perkSelectUIs[i].Init(available[i]);
        }
        for (int i = showingUICount; i < perkSelectUIs.Length; i++)
        {
            perkSelectUIs[i].gameObject.SetActive(false);
        }

        return true;
    }

    private List<int> GetAvailablePerks()
    {
        var temp = new List<int>();

        foreach (var v in Perks)
        {
            if (!activePerkIdxs.Contains(v.Key))
            {
                bool allClear = true;
                var needs = v.Value.needPerks;
                for (int i = 0; i < needs.Count; i++)
                {
                    if (!activePerkIdxs.Contains(needs[i].idx))
                    {
                        allClear = false;
                        break;
                    }
                }
                if (allClear)
                    temp.Add(v.Key);
            }
        }

        return temp;
    }
}
