using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkInfo", menuName = "GameInfos/PerkInfo")]
public class PerkInfo : ScriptableObject
{

    public int idx;
    public int tier; // ������ ������ �󸶳� �ؾ� �ϴ���
    public Sprite icon;

    // �ִ� ���� ���� Ƚ��
    public int max = 1;
    [Space(5f)]
    public List<PerkInfo> needPerks;
    [Space(5f)]
    [Header("Ư�� ȿ��")]
    public List<UnitIdx> targetUnitIdx;
    public List<Skill> addSkills;


    public void AddEffect()
    {

    }
}
