using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GM : Singleton<GM>
{

    public float gold;
    public float metal;
    public float food;
    public float research;

    private float displayGold;
    private float displayMetal;
    private float displayFood;
    private float displayResearch;

    // 계산된 결과형 변수 (각각의 주 값을 합산)
    public int pop;
    public int housingUse;
    public int housing;

    private int displayPop;

    public TextMeshProUGUI[] goldTexts;
    public TextMeshProUGUI[] goldPlusTexts;
    public TextMeshProUGUI[] metalTexts;
    public TextMeshProUGUI[] metalPlusTexts;
    public TextMeshProUGUI[] foodTexts;
    public TextMeshProUGUI[] foodPlusTexts;
    public TextMeshProUGUI[] researchTexts;

    public TextMeshProUGUI[] popTexts;
    public TextMeshProUGUI[] housingTexts;

    [Space(20f)]
    public RTSUnit[] walls;
    // 계산된 결과
    public int wallHpMax;
    public int wallHp;
    public int displayWallHp;
    public TextMeshProUGUI wallHpText;
    public Slider wallHpSlider;

    [Space(20f)]
    public float exp;
    public Slider expSilder;
    public int level;
    public TextMeshProUGUI expText;
    [SerializedDictionary("레벨", "필요누적EXP")]
    public List<float> levelExp;

    private void Start()
    {
        //// 테스트
        SetGold(60f);
        SetMetal(0);
        SetFood(0);
        SetResearch(0);
        SetExp(0);

        //// 테스트2
        //SetPop(0);
        //SetUnemployed(0);
        //SetHousing(0);

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return null;

        CalcWallHp();

        yield return new WaitForSeconds(1f);

        //AddGold(1000);
    }


    #region 자원 관련
    public void ShowPlusTexts(float gold, float metal, float food)
    {
        string goldStr;
        if (gold >= 0)
            goldStr = $"(+{gold:F0})";
        else
            goldStr = $"({gold:F0})";

        for (int i = 0; i < goldPlusTexts.Length; i++)
        {
            goldPlusTexts[i].text = goldStr;
        }

        string metalStr;
        if (metal >= 0)
            metalStr = $"(+{metal:F0})";
        else
            metalStr = $"({metal:F0})";

        for (int i = 0; i < metalPlusTexts.Length; i++)
        {
            metalPlusTexts[i].text = metalStr;
        }

        string foodStr;
        if (food >= 0)
            foodStr = $"(+{food:F0})";
        else
            foodStr = $"({food:F0})";

        for (int i = 0; i < foodPlusTexts.Length; i++)
        {
            foodPlusTexts[i].text = foodStr;
        }
    }
    public void AddGold(float value)
    {
        float target = gold + value;
        DOVirtual.Float(gold, target, 0.75f, (x) =>
        {
            displayGold = x;
            ShowGoldText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        gold = target;
    }

    private void ShowGoldText()
    {
        string str;
        if (displayGold < 0)
            str = $"<color=#A91111>{displayGold:F0}</color>";
        else
            str = $"{displayGold:F0}";

        for (int i = 0; i < goldTexts.Length; i++)
        {
            goldTexts[i].text = str;
        }
    }

    public void SetGold(float value)
    {
        displayGold = value;
        gold = value;
        ShowGoldText();
    }

    public void AddMetal(float value)
    {
        float target = metal + value;
        DOVirtual.Float(metal, target, 0.75f, (x) =>
        {
            displayMetal = x; 
            ShowMetalText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        metal = target;
    }

    private void ShowMetalText()
    {
        string str;
        if (displayMetal < 0)
            str = $"<color=#A91111>{displayMetal:F0}</color>";
        else
            str = $"{displayMetal:F0}";

        for (int i = 0; i < metalTexts.Length; i++)
        {
            metalTexts[i].text = str;
        }
    }

    public void SetMetal(float value)
    {
        displayMetal = value;
        metal = value;
        ShowMetalText();
    }

    public void AddFood(float value)
    {
        float target = food + value;
        DOVirtual.Float(food, target, 0.75f, (x) =>
        {
            displayFood = x;
            ShowFoodText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        food = target;
    }

    private void ShowFoodText()
    {
        string str;
        if (displayFood < 0)
            str = $"<color=#A91111>{displayFood:F0}</color>";
        else
            str = $"{displayFood:F0}";

        for (int i = 0; i < foodTexts.Length; i++)
        {
            foodTexts[i].text = str;
        }
    }

    public void SetFood(float value)
    {
        displayFood = value;
        food = value;
        ShowFoodText();
    }

    public void CalcResearch(float target)
    {
        DOVirtual.Float(research, target, 0.75f, (x) =>
        {
            displayResearch = x;
            ShowResearchText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        research = target;
    }

    private void ShowResearchText()
    {
        string str;
        if (displayResearch < 0)
            str = $"<color=#A91111>{displayResearch:F0}</color>";
        else
            str = $"{displayResearch:F0}";

        for (int i = 0; i < researchTexts.Length; i++)
        {
            researchTexts[i].text = str;
        }
    }

    public void SetResearch(float value)
    {
        displayResearch = value;
        research = value;
        ShowResearchText();
    }
    #endregion

    #region 결과형 자원 관련
    public void CalcPop(int target)
    {
        DOVirtual.Int(pop, target, 0.75f, (x) =>
        {
            displayPop = x;
            ShowPopText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        pop = target;
    }

    private void ShowPopText()
    {
        string str;
        if (displayPop < 0)
            str = $"<color=#A91111>{displayPop}</color>";
        else
            str = $"{displayPop}";

        for (int i = 0; i < popTexts.Length; i++)
        {
            popTexts[i].text = str;
        }
    }

    public void SetPop(int value)
    {
        displayPop = value;
        pop = value;
        ShowPopText();
    }

    public void CalcHousing(int housing, int housingUse)
    {
        this.housing = housing;
        this.housingUse = housingUse;
        ShowHousingText();
    }

    private void ShowHousingText()
    {
        string str;
        if (housingUse >= housing)
            str = $"<color=#A91111>{housingUse} / {housing}</color>";
        else
            str = $"{housingUse} / {housing}";

        for (int i = 0; i < housingTexts.Length; i++)
        {
            housingTexts[i].text = str;
        }
    }
    #endregion


    public bool CheckRes(BuildingInfo buildingInfo)
    {
        if (gold < buildingInfo.cost_gold) return false;
        if (metal < buildingInfo.cost_metal) return false;
        if (food < buildingInfo.cost_food) return false;
        return true;
    }
    public bool CheckRes(int idx)
    {
        var buildingInfos = DataManager.Instance.buildings;
        if (gold < buildingInfos[idx].cost_gold) return false;
        if (metal < buildingInfos[idx].cost_metal) return false;
        if (food < buildingInfos[idx].cost_food) return false;
        return true;
    }
    public bool CheckHousing(BuildingInfo buildingInfo)
    {
        if (housing + buildingInfo.housing < buildingInfo.housingUse + housingUse) return false;
        return true;
    }
    public bool CheckHousing(int idx)
    {
        var buildingInfos = DataManager.Instance.buildings;
        if (housing + buildingInfos[idx].housing < housingUse + buildingInfos[idx].housingUse) return false;
        return true;
    }
    public bool PayRes(BuildingInfo buildingInfo)
    {
        if (CheckRes(buildingInfo) && CheckHousing(buildingInfo))
        {
            AddGold(-1 * buildingInfo.cost_gold);
            AddMetal(-1 * buildingInfo.cost_metal);
            AddFood(-1 * buildingInfo.cost_food);
            return true;
        }
        return false;
    }

    public void ReturnRes(BuildingInfo buildingInfo)
    {
        AddGold(buildingInfo.cost_gold);
        AddMetal(buildingInfo.cost_metal);
        AddFood(buildingInfo.cost_food);
    }

    #region 성벽
    public void CalcWallHp()
    {
        float target = 0;
        float targetMax = 0;
        for (int i = 0; i < walls.Length; i++)
        {
            target += walls[i].currentHealth;
            targetMax += walls[i].Info.MaxHealth;
        }

        int targetHp = (int)target;
        int targetHpMax = (int)targetMax;

        wallHpMax = targetHpMax;
        DOVirtual.Int(wallHp, targetHp, 0.5f, (x) =>
        {
            displayWallHp = x;
            ShowWallHpText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        wallHp = targetHp;
    }

    private void ShowWallHpText()
    {
        float percent = (float)displayWallHp / wallHpMax;

        wallHpSlider.value = percent;
        wallHpText.text = displayWallHp.ToString();
    }
    #endregion

    #region 레벨
    public void SetExp(float value)
    {
        exp = value;
        ShowExpText();
    }
    public void AddExp(float addedExp)
    {
        exp += addedExp;
        int currentLevel = level;

        for (int i = currentLevel; i < levelExp.Count; i++)
        {
            if (exp >= levelExp[i])
            {
                level = i + 1;
            }
        }

        int levelUpCount = level - currentLevel;
        if (levelUpCount > 0)
        {
            // 레벨업함
            Debug.Log("Level ++ " + level);

            UIManager.Instance.OpenLevelUpPanel();

        }
        ShowExpText();
    }

    private void ShowExpText()
    {
        if (level < levelExp.Count)
        {
            if (level > 0)
            {
                float prevExp = levelExp[level - 1];
                float percent = (exp - prevExp) / (levelExp[level] - prevExp);
                expSilder.value = percent;
                expText.text = $"Lv.{level + 1}  {exp - prevExp:F0}/{levelExp[level] - prevExp:F0}";
            }
            else
            {
                float percent = exp / levelExp[level];
                expSilder.value = percent;
                expText.text = $"Lv.{level + 1}  {exp:F0}/{levelExp[level]:F0}";
            }
        }
        else
        {
            expSilder.value = 1f;
            expText.text = "Lv.Max";
        }
    }
    #endregion
}
