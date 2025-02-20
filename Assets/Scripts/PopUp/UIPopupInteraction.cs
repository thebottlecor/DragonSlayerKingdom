using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopupInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public UIPopup uiPopup;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        uiPopup.SetPopup(transform.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        uiPopup.Hide();
    }
}
