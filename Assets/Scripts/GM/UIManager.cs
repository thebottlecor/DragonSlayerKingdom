using AYellowpaper.SerializedCollections;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{


    [Header("�� ����")]
    public GameObject stateInfoPanel;
    private int currentState;


    [Header("���� ����")]
    public GameObject researchPanel;

    [Header("������ ����")]
    public GameObject levelUpPanel;

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

    public void OpenLevelUpPanel()
    {
        bool perkAvailable = PerkManager.Instance.OpenPerkUI();

        if (perkAvailable)
        {
            levelUpPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else // ���� ������ ���� �ϳ��� ����
        {
            CloseLevelUpPanel();
        }
    }
    public void SelectPerks()
    {
        CloseLevelUpPanel();
    }
    public void CloseLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
