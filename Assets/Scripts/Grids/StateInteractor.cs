using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.EventSystems;
using System;

public class StateInteractor : MonoBehaviour
{

    [SerializeField] private int stateIdx;

    [SerializeField] private GameObject fillRect;

    private void Awake()
    {
        if (fillRect == null) return;

        ResetFillRect();
    }

    public void ShowFillRect()
    {
        fillRect.SetActive(true);
    }

    public void ResetFillRect()
    {
        fillRect.SetActive(false);
    }

    private void OnMouseEnter()
    {
        ShowFillRect();
    }

    private void OnMouseExit()
    {
        ResetFillRect();
    }

    private void OnMouseUpAsButton()
    {
        UIManager.Instance.ToggleStateInfoPanel(stateIdx);
    }
}
