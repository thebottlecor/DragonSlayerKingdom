using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillInfo", menuName = "GameInfos/SkillInfo")]
public class SkillInfo : ScriptableObject
{

    public Skill idx;
    public float chances;
    public float[] dataValue;
    public float[] hiddenValue;


    public bool ChanceCheck(HashSet<Skill> SkillSet)
    {
        if (idx == Skill.None) return true;

        if (SkillSet.Contains(idx))
        {
            if (chances <= 0) return true;
            float random = UnityEngine.Random.Range(0, 100f);
            if (random <= chances)
            {
                return true;
            }
        }
        return false;
    }
}
