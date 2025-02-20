using AYellowpaper.SerializedCollections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("연구 정보")]
    public GameObject researchPanel;

    [Header("레벨업 정보")]
    public GameObject levelUpPanel;

    [Header("메뉴 정보")]
    public GameObject menuPanel;
    public TextMeshProUGUI[] menuPanelTexts;
    public Button[] toLobbyButtons;

    [Header("건설 정보")]
    public GameObject buildingPanel;
    public List<BuildingButtonUI> buildingButtonUIs;
    public BuildingParamUIPopup buildingUIPopup;


    public GameObject bottomTooltipObj;
    public TextMeshProUGUI bottomTooltipText;
    private TextManager tm => TextManager.Instance;

    private void Start()
    {
        CloseResearchPanel();
        InitBuildingUIs();


        menuPanelTexts[0].text = tm.GetCommons("Resume");
        menuPanelTexts[1].text = tm.GetCommons("Main Menu");
        menuPanelTexts[2].text = tm.GetCommons("Quit");
        menuPanelTexts[3].text = tm.GetCommons("Main Menu");
        menuPanelTexts[4].text = tm.GetCommons("Main Menu");

        for (int i = 0; i < toLobbyButtons.Length; i++)
        {
            toLobbyButtons[i].onClick.AddListener(() =>
            {
                LoadingSceneManager.Instance.ToLobby();
            });
        }
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
        else // 선택 가능한 퍽이 하나도 없음
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


    public void InitBuildingUIs()
    {
        for (int i = 0; i < buildingButtonUIs.Count; i++)
        {
            var ui = buildingButtonUIs[i];
            ui.popupInteraction.uiPopup = buildingUIPopup;
            ui.UpdateUI();
            ui.GetComponent<Button>().onClick.AddListener(() =>
            {
                GridBuildingSystem.Instance.InitWithBuilding(ui.Idx);
            });
        }
    }
    public void ToggleBuildingPanel()
    {
        if (buildingPanel.activeSelf)
            CloseBuildingPanel();
        else
            OpenBuildingPanel();
    }
    public void OpenBuildingPanel()
    {
        buildingPanel.SetActive(true);
    }
    public void CloseBuildingPanel()
    {
        buildingPanel.SetActive(false);
        buildingUIPopup.Hide();
    }


    public void BottomTooltipOpen(BottomTooltipType type)
    {
        switch (type)
        {
            case BottomTooltipType.build:
                bottomTooltipText.text = tm.GetCommons("BuildTooltip");
                break;
        }
        bottomTooltipObj.SetActive(true);
    }
    public void BottomTooltipClose()
    {
        bottomTooltipObj.SetActive(false);
    }


    public void ToggleMenuPanel()
    {
        if (menuPanel.activeSelf)
            CloseMenuPanel();
        else
            OpenMenuPanel();
    }
    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseMenuPanel()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public enum BottomTooltipType
{
    build,
}
