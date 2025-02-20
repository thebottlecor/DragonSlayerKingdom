using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System;

public class PerkSelectUI : MonoBehaviour
{

    public int showingIdx;

    public Image iconBG;
    public Image icon;

    public TextMeshProUGUI description;

    public Button selectBtn;
    public TextMeshProUGUI btnText;


    public void Init(int idx)
    {
        showingIdx = idx;
        UpdateUI();
    }

    public void UpdateUI()
    {
        var perk = DataManager.Instance.perks[showingIdx];
        icon.sprite = perk.icon;

        var tm = TextManager.Instance;
        StringBuilder st = new StringBuilder();
        st.Append(tm.GetPerk(showingIdx));
        st.AppendLine();
        st.Append("<size=75%>");

        Skill firstSkill = perk.addSkills[0];
        //Debug.Log(showingIdx + " >> " + firstSkill);

        if (firstSkill != Skill.None)
        {
            SkillInfo skillInfo = DataManager.Instance.skills[firstSkill];

            if (skillInfo.chances > 0f)
            {
                if (skillInfo.dataValue != null && skillInfo.dataValue.Length > 0)
                {
                    float[] temp2 = new float[skillInfo.dataValue.Length + 1];
                    temp2[0] = skillInfo.chances;
                    for (int i = 1; i < temp2.Length; i++)
                        temp2[i] = skillInfo.dataValue[i - 1];
                    object[] temp = Array.ConvertAll(temp2, x => (object)x);
                    //Debug.Log(tm.GetPerkDetail(showingIdx) + " >> " + temp.Length + " >>>" + skillInfo.dataValue.Length);
                    st.AppendFormat(tm.GetPerkDetail(showingIdx), temp);
                }
                else
                    st.AppendFormat(tm.GetPerkDetail(showingIdx), skillInfo.chances);

            }
            else
            {
                if (skillInfo.dataValue != null && skillInfo.dataValue.Length > 0)
                {
                    object[] temp = Array.ConvertAll(skillInfo.dataValue, x => (object)x);
                    //Debug.Log(tm.GetPerkDetail(showingIdx) + " >> " + temp.Length + " >>>" + skillInfo.dataValue.Length);
                    st.AppendFormat(tm.GetPerkDetail(showingIdx), temp);
                }
                else
                    st.Append(tm.GetPerkDetail(showingIdx));
            }
        }

        st.AppendLine();
        st.AppendLine();
        st.Append(tm.GetCommons("AffectedUnits"));
        st.AppendLine();
        for (int i = 0; i < perk.targetUnitIdx.Count; i++)
        {
            st.Append(tm.GetUnit((int)perk.targetUnitIdx[i]));
            if (i > 0 && i < perk.targetUnitIdx.Count - 1)
                st.Append(",");
        }

        st.Append("</size>");

        description.text = st.ToString();

        btnText.text = tm.GetCommons("Select");
    }

    public void SelectThis()
    {
        PerkManager.Instance.AddPerk(showingIdx);
        UIManager.Instance.SelectPerks();
    }
}
