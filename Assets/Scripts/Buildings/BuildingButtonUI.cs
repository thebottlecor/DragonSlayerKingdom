using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BuildingButtonUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI tmp;
    public BuildingParamPopupInteraction popupInteraction;
    public int Idx => popupInteraction.buildingIdx;

    public void UpdateUI()
    {
        var info = DataManager.Instance.buildings[Idx];

        icon.sprite = info.icon;
        tmp.text = TextManager.Instance.GetBuilding(Idx);
    }
}
