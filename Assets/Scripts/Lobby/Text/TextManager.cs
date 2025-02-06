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
    ru,
    LAST,
}

public class TextManager : Singleton<TextManager>
{

    public Language language;

    Dictionary<int, Dictionary<string, object>> researches;
    public string GetResearch(int idx) => researches[idx][language.ToString()].ToString();

    Dictionary<string, Dictionary<string, object>> commons;
    public string GetCommons(string str) => commons[str][language.ToString()].ToString();
    public string GetCommons(string str, Language lan) => commons[str][lan.ToString()].ToString();

    Dictionary<KeyCode, Dictionary<string, object>> keycodes;
    public string GetKeyCodes(KeyCode idx) => keycodes[idx]["all"].ToString();
    public bool HasKeyCode(KeyCode idx) => keycodes.ContainsKey(idx);

    Dictionary<KeyCode, Dictionary<string, object>> inputSystems;
    public string GetInputSystems(KeyCode idx) => inputSystems[idx]["all"].ToString();
    public bool HasInputSystems(KeyCode idx) => inputSystems.ContainsKey(idx);

    Dictionary<int, Dictionary<string, object>> characters;
    public string GetSurvivorName(int idx) => characters[idx][language.ToString()].ToString();

    Dictionary<int, Dictionary<string, object>> villagers;
    public string GetVillagerName(int idx) => villagers[idx][language.ToString()].ToString();

    Dictionary<int, Dictionary<string, object>> vehicles;
    public string GetVehicles(int idx) => vehicles[idx][language.ToString()].ToString();

    Dictionary<int, Dictionary<string, object>> spaceships;
    public string GetSpaceships(int idx) => spaceships[idx][language.ToString()].ToString();

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
            case SystemLanguage.Russian:
                firstLanguage = Language.ru;
                break;
        }
        SetLanguage(firstLanguage);
    }

    public void SetLanguage(Language language)
    {
        this.language = language;

        researches = CSVReader.ReadCSV<int>("TextManager - research.csv");
        commons = CSVReader.ReadCSV<string>("TextManager - common.csv");
        keycodes = CSVReader.ReadCSV<KeyCode>("TextManager - keycode.csv");
        inputSystems = CSVReader.ReadCSV<KeyCode>("TextManager - inputSystem.csv");
        characters = CSVReader.ReadCSV<int>("TextManager - character.csv");
        villagers = CSVReader.ReadCSV<int>("TextManager - villager.csv");
        vehicles = CSVReader.ReadCSV<int>("TextManager - vehicle.csv");
        spaceships = CSVReader.ReadCSV<int>("TextManager - spaceship.csv");

        //Lobby.Instance.UpdateTexts();
        //SettingManager.Instance.UpdateTexts();
        //UINaviHelper.Instance.UpdateTexts();
        //UINaviHelper.Instance.inputHelper.GuidePadCheck();

        if (TextChangedEvent != null)
            TextChangedEvent(null, null);
    }
}
