using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{

    public TextMeshProUGUI tmp;

    public RectTransform panel;

    public Vector2 margin;
    public Vector2 offset;

    public int activeCount;

    public bool mouseYposFixed;

    public bool adjust;
    public float adjust_offset = 41f;

    public virtual void SetPopup(Vector3 pos)
    {
        if (activeCount <= 0) activeCount = 0;
        activeCount++;
        gameObject.SetActive(true);
        transform.position = pos;

        tmp.text = string.Empty;

        SizeFitter(pos);
    }

    public virtual void SizeFitter(Vector3 pos)
    {
        panel.sizeDelta = tmp.rectTransform.sizeDelta + margin;

        // 카메라 판별
        //float differ = Camera.main.transform.position.x - pos.x;

        if (!mouseYposFixed)
        {
            //panel.anchoredPosition = new Vector2(panel.sizeDelta.x * 0.5f * (differ < 0 ? -1f : 1f), panel.sizeDelta.y * 0.5f) + offset;
            panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * 0.5f) + offset;
        }
        else
        {

            bool touchBottom = pos.y - panel.sizeDelta.y - 16f < 0f;
            if (touchBottom)
            {
                panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * 0.5f) + offset;
            }
            else
            {
                panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * -0.5f) + offset;
            }
        }
    }

    protected void SizeFitter_Adjust(Vector3 pos)
    {
        panel.sizeDelta = tmp.rectTransform.sizeDelta + margin;

        offset.x = tmp.rectTransform.sizeDelta.x - adjust_offset;

        if (!mouseYposFixed)
        {
            //panel.anchoredPosition = new Vector2(panel.sizeDelta.x * 0.5f * (differ < 0 ? -1f : 1f), panel.sizeDelta.y * 0.5f) + offset;
            panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * 0.5f) + offset;
        }
        else
        {

            bool touchBottom = pos.y - panel.sizeDelta.y - 16f < 0f;
            if (touchBottom)
            {
                panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * 0.5f) + offset;
            }
            else
            {
                panel.anchoredPosition = new Vector2(panel.sizeDelta.x * -0.5f, panel.sizeDelta.y * -0.5f) + offset;
            }
        }
    }

    public void Hide()
    {
        activeCount--;
        if (activeCount <= 0) gameObject.SetActive(false);
    }
}
