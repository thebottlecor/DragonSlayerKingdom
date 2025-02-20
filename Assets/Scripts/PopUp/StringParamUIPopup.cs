using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StringParamUIPopup : UIPopup
{
    [HideInInspector] public string currentActive;

    public void SetPopup(Vector3 pos, string str)
    {
        if (activeCount <= 0) activeCount = 0;
        activeCount++;

        //if (TileGridManager.Instance.clickMode == TileGridManager.ClickMode.normal)
        {
            gameObject.SetActive(true);
            transform.position = pos;

            StringBuilder st = new StringBuilder();
            var tm = TextManager.Instance;

            st.Append(tm.GetCommons(str));

            tmp.text = st.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(tmp.transform as RectTransform);

            if (adjust)
                SizeFitter_Adjust(pos);
            else
                SizeFitter(pos);
        }
    }
}
