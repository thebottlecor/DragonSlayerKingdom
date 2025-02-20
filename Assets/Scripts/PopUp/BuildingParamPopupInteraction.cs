using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingParamPopupInteraction : UIPopupInteraction
{
    public int buildingIdx;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        BuildingParamUIPopup pop = uiPopup as BuildingParamUIPopup;
        pop.SetPopup(transform.position, buildingIdx);
        pop.currentActive = buildingIdx;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        uiPopup.Hide();
    }
}
