using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public enum Language
{
    sc,
    tc,
    en,
    jp,
    kr,
    LAST,
}

public class TextManager : Singleton<TextManager>
{

    public Language language;

    Dictionary<int, Dictionary<string, object>> perks;
    public string GetPerk(int idx) => perks[idx][language.ToString()].ToString();
    Dictionary<int, Dictionary<string, object>> perkDetails;
    public string GetPerkDetail(int idx) => perkDetails[idx][language.ToString()].ToString();
    Dictionary<int, Dictionary<string, object>> buildings;
    public string GetBuilding(int idx) => buildings[idx][language.ToString()].ToString();
    Dictionary<int, Dictionary<string, object>> buildingDetails;
    public string GetBuildingDetail(int idx) => buildingDetails[idx][language.ToString()].ToString();
    Dictionary<int, Dictionary<string, object>> units;
    public string GetUnit(int idx) => units[idx][language.ToString()].ToString();
    Dictionary<int, Dictionary<string, object>> unitDetails;
    public string GetUnitDetail(int idx) => unitDetails[idx][language.ToString()].ToString();



    Dictionary<string, Dictionary<string, object>> commons;
    public string GetCommons(string str) => commons[str][language.ToString()].ToString();
    public string GetCommons(string str, Language lan) => commons[str][lan.ToString()].ToString();

    Dictionary<KeyCode, Dictionary<string, object>> keycodes;
    public string GetKeyCodes(KeyCode idx) => keycodes[idx]["all"].ToString();
    public bool HasKeyCode(KeyCode idx) => keycodes.ContainsKey(idx);

    public CultureInfo defaultCultureInfo = new CultureInfo("en-US");

    public static readonly int WalkId = Animator.StringToHash("Walk");
    public static readonly int AttackId = Animator.StringToHash("Attack");
    public static readonly int ContactId = Animator.StringToHash("Contact");
    public static readonly int RunId = Animator.StringToHash("Run");

    public static EventHandler TextChangedEvent;

    protected override void Awake()
    {
        base.Awake();
        //CallAfterAwake();
    }

    private void Start()
    {
        SetFirstLanguage();
    }

    //public override void CallAfterAwake()
    //{

    //}
    //public override void CallAfterStart()
    //{
    //    if (config == null)
    //    {
    //        SetFirstLanguage();
    //    }
    //    else
    //    {
    //        if (System.Enum.TryParse(typeof(Language), config.lan, out object result))
    //        {
    //            SetLanguage((Language)result);
    //        }
    //        else
    //            SetFirstLanguage();
    //    }
    //}

    private void SetFirstLanguage()
    {
        Language firstLanguage = Language.en;
        var systemLanguage = Application.systemLanguage;
        switch (systemLanguage)
        {
            case SystemLanguage.ChineseTraditional:
                firstLanguage = Language.tc;
                break;
            case SystemLanguage.ChineseSimplified:
                firstLanguage = Language.sc;
                break;
            case SystemLanguage.Chinese:
                firstLanguage = Language.sc;
                break;
            case SystemLanguage.Japanese:
                firstLanguage = Language.jp;
                break;
            case SystemLanguage.Korean:
                firstLanguage = Language.kr;
                break;
        }
        SetLanguage(firstLanguage);
    }

    public void SetLanguage(Language language)
    {
        this.language = language;

        perks = CSVReader.ReadCSV<int>("TextManager - perk.csv");
        perkDetails = CSVReader.ReadCSV<int>("TextManager - perkDetail.csv");
        buildings = CSVReader.ReadCSV<int>("TextManager - building.csv");
        buildingDetails = CSVReader.ReadCSV<int>("TextManager - buildingDetail.csv");
        units = CSVReader.ReadCSV<int>("TextManager - unit.csv");
        unitDetails = CSVReader.ReadCSV<int>("TextManager - unitDetail.csv");
        commons = CSVReader.ReadCSV<string>("TextManager - common.csv");
        keycodes = CSVReader.ReadCSV<KeyCode>("TextManager - keycode.csv");

        //Lobby.Instance.UpdateTexts();
        //SettingManager.Instance.UpdateTexts();
        //UINaviHelper.Instance.UpdateTexts();
        //UINaviHelper.Instance.inputHelper.GuidePadCheck();

        if (TextChangedEvent != null)
            TextChangedEvent(null, null);
    }
}
