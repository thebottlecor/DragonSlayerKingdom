using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    }

    public void SelectThis()
    {
        UIManager.Instance.SelectPerks(showingIdx);
    }
}
