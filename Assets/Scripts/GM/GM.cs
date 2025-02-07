using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;

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
    public int unemployed;
    public int housing;

    private int displayPop;
    private int displayUnemployed;
    private int displayHousing;

    public TextMeshProUGUI[] goldTexts;
    public TextMeshProUGUI[] metalTexts;
    public TextMeshProUGUI[] foodTexts;
    public TextMeshProUGUI[] researchTexts;

    public TextMeshProUGUI[] popTexts;
    public TextMeshProUGUI[] unemployedTexts;
    public TextMeshProUGUI[] housingTexts;

    public RTSUnit[] walls;
    // 계산된 결과
    public int wallHpMax;
    public int wallHp;
    public int displayWallHp;
    public TextMeshProUGUI wallHpText;
    public Slider wallHpSlider;

    private void Start()
    {
        // 테스트
        SetGold(500);
        SetMetal(500);
        SetFood(500);
        SetResearch(0);

        // 테스트2
        SetPop(0);
        SetUnemployed(0);
        SetHousing(0);

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return null;

        CalcWallHp();

        yield return new WaitForSeconds(1f);

        AddGold(1000);
    }


    #region 자원 관련
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

    public void CalcUnemployed(int target) // 실제로 일자리 여유분을 나타냄
    {
        DOVirtual.Int(unemployed, target, 0.75f, (x) =>
        {
            displayUnemployed = x;
            ShowUnemployedText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        unemployed = target;
    }

    private void ShowUnemployedText()
    {
        string str;
        if (displayUnemployed < 0)
            str = $"<color=#A91111>{displayUnemployed}</color>";
        else
            str = $"{displayUnemployed}";

        for (int i = 0; i < unemployedTexts.Length; i++)
        {
            unemployedTexts[i].text = str;
        }
    }

    public void SetUnemployed(int value)
    {
        displayUnemployed = value;
        unemployed = value;
        ShowUnemployedText();
    }

    public void CalcHousing(int target)
    {
        DOVirtual.Int(housing, target, 0.75f, (x) =>
        {
            displayHousing = x;
            ShowHousingText();
        }).SetEase(Ease.OutCirc).SetUpdate(true);
        housing = target;
    }

    private void ShowHousingText()
    {
        string str;
        if (displayHousing < 0)
            str = $"<color=#A91111>{displayHousing}</color>";
        else
            str = $"{displayHousing}";

        for (int i = 0; i < housingTexts.Length; i++)
        {
            housingTexts[i].text = str;
        }
    }

    public void SetHousing(int value)
    {
        displayHousing = value;
        housing = value;
        ShowHousingText();
    }
    #endregion


    public bool CheckRes(BuildingInfo buildingInfo)
    {
        if (gold < buildingInfo.cost_gold) return false;
        if (metal < buildingInfo.cost_metal) return false;
        if (food < buildingInfo.cost_food) return false;
        return true;
    }
    public bool PayRes(BuildingInfo buildingInfo)
    {
        if (CheckRes(buildingInfo))
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
            target += walls[i].health;
            targetMax += walls[i].maxHealth;
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
}
