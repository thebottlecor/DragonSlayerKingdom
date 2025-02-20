using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StringParamPopupInteraction : UIPopupInteraction
{
    public string param;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        StringParamUIPopup pop = uiPopup as StringParamUIPopup;
        pop.SetPopup(transform.position, param);
        pop.currentActive = param;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        uiPopup.Hide();
    }
}
