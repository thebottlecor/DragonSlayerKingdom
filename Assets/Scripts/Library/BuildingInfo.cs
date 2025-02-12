using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "BuildingInfo", menuName = "GameInfos/BuildingInfo")]
public class BuildingInfo : ScriptableObject
{
    public Building prefab;
    public bool indestructible;

    [Header("속성")]
    public float product_gold;
    public float product_metal;
    public float product_food;
    public float product_research;

    public float growth_rating;

    public int housingUse;
    public int housing;

    [Header("스폰")]
    public GameObject spawnUnit;
    public int spawnCount = 1;

    [Header("건설 비용")]
    public float cost_gold;
    public float cost_metal;
    public float cost_food;
    public float build_time;

}
