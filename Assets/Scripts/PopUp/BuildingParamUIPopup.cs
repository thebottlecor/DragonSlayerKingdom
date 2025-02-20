using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingParamUIPopup : UIPopup
{
    [HideInInspector] public int currentActive;

    public void SetPopup(Vector3 pos, int buildingIdx)
    {
        if (activeCount <= 0) activeCount = 0;
        activeCount++;

        //if (TileGridManager.Instance.clickMode == TileGridManager.ClickMode.normal)
        {
            BuildingInfo info = BuildingManager.Instance.BuildingInfos[buildingIdx];

            gameObject.SetActive(true);
            transform.position = pos;

            StringBuilder st = new StringBuilder();
            var tm = TextManager.Instance;

            st.Append("<size=125%>");
            st.Append(tm.GetBuilding(buildingIdx));
            st.Append("</size>");

            if (info.cost_gold > 0 || info.cost_food > 0 || info.cost_metal > 0 || info.housingUse > 0)
            {
                st.AppendLine();
                st.Append(" ").Append(tm.GetCommons("Costs"));
                if (info.cost_gold > 0)
                    st.Append("<sprite=0>").Append(info.cost_gold);
                if (info.cost_food > 0)
                    st.Append("<sprite=1>").Append(info.cost_food);
                if (info.cost_metal > 0)
                    st.Append("<sprite=19>").Append(info.cost_metal);
                if (info.housingUse > 0)
                    st.Append("<sprite=45>").Append(info.housingUse);
            }

            //st.AppendLine();
            //st.Append(tm.GetBuildingDetail(buildingIdx));


            if (info.product_gold > 0 || info.product_food > 0 || info.product_metal > 0 || info.housing > 0)
            {
                st.AppendLine();
                st.Append(" ").Append(tm.GetCommons("Production"));
                if (info.product_gold > 0)
                    st.Append("<sprite=0>").Append(info.product_gold);
                if (info.product_food > 0)
                    st.Append("<sprite=1>").Append(info.product_food);
                if (info.product_metal > 0)
                    st.Append("<sprite=19>").Append(info.product_metal);
                if (info.housing > 0)
                    st.Append("<sprite=45>").Append(info.housing);
            }
            if (info.product_gold < 0 || info.product_food < 0 || info.product_metal < 0)
            {
                st.AppendLine();
                st.Append(" ").Append(tm.GetCommons("Upkeep"));
                if (info.product_gold < 0)
                    st.Append("<sprite=0>").Append(-1 * info.product_gold);
                if (info.product_food < 0)
                    st.Append("<sprite=1>").Append(-1 * info.product_food);
                if (info.product_metal < 0)
                    st.Append("<sprite=19>").Append(-1 * info.product_metal);
            }

            if (info.spawnUnit != null)
            {
                var idx = info.spawnUnit.GetComponent<RTSUnit>().idx;
                var unitInfo = DataManager.Instance.units[idx];

                st.AppendLine();
                st.Append(" ").Append(tm.GetCommons("TrainingUnits"));
                st.AppendLine();
                st.Append(" <size=90%>>").Append(tm.GetUnit((int)idx));
                st.Append(" <sprite=42>").Append(unitInfo.MaxHealth);
                st.Append(" <sprite=51>").Append(unitInfo.Damage);
                //st.Append(" <sprite=50>").Append(unitInfo.Armor);
                st.Append("</size>");
            }
            

            tmp.text = st.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(tmp.transform as RectTransform);

            if (adjust)
                SizeFitter_Adjust(pos);
            else
                SizeFitter(pos);
        }
    }
}
