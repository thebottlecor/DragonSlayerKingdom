using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkInfo", menuName = "GameInfos/PerkInfo")]
public class PerkInfo : ScriptableObject
{

    public int idx;
    public int tier; // 레시피 연구를 얼마나 해야 하는지
    public Sprite icon;

    // 최대 연구 가능 횟수
    public int max = 1;
    [Space(5f)]
    public List<PerkInfo> needPerks;
    [Space(5f)]
    [Header("특전 효과")]
    public List<UnitIdx> targetUnitIdx;
    public List<Skill> addSkills;


    public void AddEffect()
    {

    }
}
