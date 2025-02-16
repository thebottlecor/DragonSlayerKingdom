using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UILibrary uiLib;
    public MaterialLibrary materialLib;

    [SerializeField] private PerkLibrary perkLib;
    public Dictionary<int, PerkInfo> perks;

    [SerializeField] private UpgradeLibrary upgradeLib;
    public Dictionary<int, UpgradeInfo> upgrades;

    [SerializeField] private BuildingLibrary buildingLib;
    public Dictionary<int, BuildingInfo> buildings;

    [SerializeField] private UnitLibrary unitLib;
    public Dictionary<UnitIdx, UnitInfo> units;

    protected override void Awake()
    {
        base.Awake();

        //uiLib.padKeyUIsXbox = new SerializableDictionary<PadKeyCode, Sprite>();
        //uiLib.padKeyUIsPS = new SerializableDictionary<PadKeyCode, Sprite>();
        //foreach (var tt in uiLib.padKeyUIs)
        //{
        //    SerializableDictionary<PadKeyCode, Sprite>.Pair tqq = new SerializableDictionary<PadKeyCode, Sprite>.Pair(tt.Key, tt.Value);

        //    uiLib.padKeyUIsXbox.Add(tqq);
        //    uiLib.padKeyUIsPS.Add(tqq);
        //}
    }

    private void Start()
    {
        perks = perkLib.GetHashMap();
        upgrades = upgradeLib.GetHashMap();
        buildings = buildingLib.GetHashMap();
        foreach (var v in buildings)
        {
            v.Value.prefab.Set_Idx(v.Key);
        }
        units = unitLib.GetHashMap();
        foreach (var v in units)
        {
            v.Value.CalcSomeValue();
            v.Value.prefab.Set_Idx(v.Key);
        }
        DOTween.Init();
    }
}
