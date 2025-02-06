using AYellowpaper.SerializedCollections;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{


    [Header("주 정보")]
    public GameObject stateInfoPanel;
    private int currentState;


    [Header("연구 정보")]
    public GameObject researchPanel;

    [SerializedDictionary("이게뭐여?", "이것은 말이지")]
    public SerializedDictionary<int, string> ElementDescriptions;

    private void Start()
    {
        CloseStateInfoPanel();
        CloseResearchPanel();
    }


    public void ToggleStateInfoPanel(int stateIdx)
    {
        if (stateInfoPanel.activeSelf)
        {
            if (currentState != stateIdx)
                OpenStateInfoPanel(stateIdx);
            else
                CloseStateInfoPanel();
        }
        else
            OpenStateInfoPanel(stateIdx);
    }

    public void OpenStateInfoPanel(int stateIdx)
    {
        Debug.Log("Open State :: " + stateIdx);

        currentState = stateIdx;

        stateInfoPanel.SetActive(true);
    }
    public void CloseStateInfoPanel()
    {
        stateInfoPanel.SetActive(false);

        currentState = -1;
    }

    public void ToggleResearchPanel()
    {
        if (researchPanel.activeSelf)
            CloseResearchPanel();
        else
            OpenResearchPanel();
    }

    public void OpenResearchPanel()
    {
        researchPanel.SetActive(true);
    }
    public void CloseResearchPanel()
    {
        researchPanel.SetActive(false);
    }
}
