using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class BuildingManager : Singleton<BuildingManager>
{
    [Serializable]
    public struct ProductInfo
    {
        public float product_gold;
        public float product_metal;
        public float product_food;
        public float product_research;

        public int pop;
        public float growth_rating;
        public int growth_pop;

        public int housingUse;
        public int housing;
    }

    [Header("설정값")]
    public float baseGrowthRating = 0.01f;

    [Header("게임 변수")]
    public ProductInfo productInfos;
    public SerializedDictionary<int, int> hasBuildings;

    public Queue<BuildingQueueInfo> buildingQueue;
    [Serializable]
    public struct BuildingQueueInfo
    {
        public int buildingIdx;
    }
    public BuildingQueueInfo currentBuildingInfo;
    public float currentBuildingTimer;

    public float total_product_gold;
    public float total_product_metal;
    public float total_product_food;
    public float total_product_research;
    public int total_pop;
    public int total_growth_pop;
    public int total_housing;
    public int total_housingUse;

    public List<Building> playerBuildings;
    public List<SpawnInfo> spawnPool_up;
    public List<SpawnInfo> spawnPool;
    public List<SpawnInfo> spawnPool_down;
    [Serializable]
    public struct SpawnInfo
    {
        /// <summary> 위치로 스폰 순서를 정함, 우측이 먼저 생성되어 전방을 담당 </summary>
        public Vector2 sourcePos;
        /// <summary> 스폰 건물의 Y 위치에 따라 초기 스폰되는 Y 위치는 상이하게 달라짐 </summary>
        public int sortY;
        public GameObject obj;
        public int count;
    }

    // 특정 시간마다 더해지는 주기당 보너스 골드
    public float bonus_gold;

    public Dictionary<int, BuildingInfo> BuildingInfos => DataManager.Instance.buildings;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        productInfos = new ProductInfo();

        hasBuildings = new SerializedDictionary<int, int>();
        foreach (var v in BuildingInfos)
        {
            hasBuildings.Add(v.Key, 0);
        }

        buildingQueue = new Queue<BuildingQueueInfo>();
        currentBuildingTimer = int.MinValue;

        // 임시 초기 설정
        var temp = Instantiate(BuildingInfos[0].prefab, Vector3.zero, Quaternion.identity);
        temp.Init();
        temp.transform.position = new Vector3(-100.125f, 1.625f, 0f);
        temp.Place();
        productInfos.pop = 1000;

        YieldState(false);
    }

    private void Update()
    {
        if (currentBuildingTimer > 0)
        {
            currentBuildingTimer += -1f * Time.deltaTime;
        }
        else
        {
            if (currentBuildingTimer == int.MinValue)
            {
                if (buildingQueue.Count > 0)
                {
                    var temp = buildingQueue.Dequeue();
                    currentBuildingInfo = temp;
                    currentBuildingTimer = BuildingInfos[temp.buildingIdx].build_time;
                    Debug.Log("건설 대기열 불러옴 " + currentBuildingInfo.buildingIdx);
                }
            }
            else
            {
                BuildSomething(currentBuildingInfo.buildingIdx);
                currentBuildingTimer = int.MinValue;
                Debug.Log("건설 완료 " + currentBuildingInfo.buildingIdx);
            }
        }
    }


    public void BuildSomethingByBtn(int buildingIdx)
    {
        if (GM.Instance.PayRes(BuildingInfos[buildingIdx]))
        {
            BuildingQueueInfo queueInfo = new BuildingQueueInfo { buildingIdx = buildingIdx };
            if (currentBuildingTimer == int.MinValue)
            {
                currentBuildingInfo = queueInfo;
                currentBuildingTimer = BuildingInfos[buildingIdx].build_time;
            }
            else
            {
                buildingQueue.Enqueue(queueInfo);
            }
        }
    }

    public void BuildSomething(int buildingIdx, int count = 1)
    {
        hasBuildings[buildingIdx] += count;
        //CalcState(stateIdx);
        YieldState(false);
    }

    public void CalcState()
    {
        ProductInfo temp = productInfos;

        float product_gold = 0;
        float product_metal = 0;
        float product_food = 0;
        float product_research = 0;

        float growth_rating = 0;
        int jobs = 0;
        int housing = 0;

        foreach (var v in hasBuildings)
        {
            int count = v.Value;
            var stat = BuildingInfos[v.Key];

            product_gold += stat.product_gold * count;
            product_metal += stat.product_metal * count;
            product_food += stat.product_food * count;
            product_research += stat.product_research * count;

            growth_rating += stat.growth_rating * count;
            jobs += stat.housingUse * count;
            housing += stat.housing * count;
        }

        temp.product_gold = product_gold;
        temp.product_metal = product_metal;
        temp.product_food = product_food;
        temp.product_research = product_research;

        temp.housingUse = jobs;
        temp.housing = housing;

        float calcGrowthRating = baseGrowthRating + growth_rating;
        temp.growth_rating = calcGrowthRating;

        float growth_pop = temp.pop * calcGrowthRating;
        growth_pop = Mathf.Max(growth_pop, 1f);
        temp.growth_pop = (int)growth_pop;

        productInfos = temp;
    }

    public void AddPop()
    {
        ProductInfo temp = productInfos;
        temp.pop += temp.growth_pop;
        productInfos = temp;
    }


    public void YieldState(bool addRes)
    {
        total_product_gold = 0;
        total_product_metal = 0;
        total_product_food = 0;
        total_product_research = 0;
        total_pop = 0;
        total_growth_pop = 0;
        total_housing = 0;
        total_housingUse = 0;

        CalcState();
        if (addRes)
            AddPop();

        total_product_gold += productInfos.product_gold;
        total_product_metal += productInfos.product_metal;
        total_product_food += productInfos.product_food;
        total_product_research += productInfos.product_research;

        total_product_gold += bonus_gold;

        total_pop += productInfos.pop;
        total_growth_pop += productInfos.growth_pop;
        total_housing += productInfos.housing;
        total_housingUse += productInfos.housingUse;

        if (addRes)
        {
            GM.Instance.AddGold(total_product_gold);
            GM.Instance.AddMetal(total_product_metal);
            GM.Instance.AddFood(total_product_food);
        }
        GM.Instance.ShowPlusTexts(total_product_gold, total_product_metal, total_product_food);
        GM.Instance.CalcResearch(total_product_research);

        GM.Instance.CalcPop(total_pop);
        GM.Instance.CalcHousing(total_housing, total_housingUse);
    }

    public bool AddPlayerBuilding(Building building)
    {
        int buildingIdx = building.idx;

        if (GM.Instance.PayRes(BuildingInfos[buildingIdx]))
        {
            //BuildingQueueInfo queueInfo = new BuildingQueueInfo { buildingIdx = buildingIdx };
            //if (currentBuildingTimer == int.MinValue)
            //{
            //    currentBuildingInfo = queueInfo;
            //    currentBuildingTimer = buildingInfos[buildingIdx].build_time;
            //}
            //else
            //{
            //    buildingQueue.Enqueue(queueInfo);
            //}

            playerBuildings.Add(building);
            BuildSomething(buildingIdx);

            return true;
        }
        return false;
    }
    public void RemovePlayerBuilding(Building building)
    {
        int buildingIdx = building.idx;
        playerBuildings.Remove(building);
        BuildSomething(buildingIdx, -1);
    }


    public void UpdateSpawnPool()
    {
        spawnPool = new List<SpawnInfo>();
        spawnPool_up = new List<SpawnInfo>();
        spawnPool_down = new List<SpawnInfo>();

        for (int i = 0; i < playerBuildings.Count; i++)
        {
            // 스폰 체크
            var su = BuildingInfos[playerBuildings[i].idx].spawnUnit;
            int count = BuildingInfos[playerBuildings[i].idx].spawnCount;
            if (su != null)
            {
                Vector3 pos = playerBuildings[i].center.position;
                SpawnInfo spawnInfo = new SpawnInfo
                {
                    sourcePos = pos,
                    obj = su,
                    count = count,
                };

                if (pos.y >= 2f)
                {
                    spawnPool_up.Add(spawnInfo);
                }
                else if (pos.y > 1.5f)
                {
                    spawnPool.Add(spawnInfo);
                }
                else
                {
                    spawnPool_down.Add(spawnInfo);
                }
            }
        }

        var sortedPool = from v in spawnPool
                         orderby v.sourcePos.x descending
                         select v;
        var sortedPool_up = from v in spawnPool_up
                            orderby v.sourcePos.x descending
                            select v;
        var sortedPool_down = from v in spawnPool_down
                              orderby v.sourcePos.x descending
                            select v;

        spawnPool = sortedPool.ToList();
        spawnPool_up = sortedPool_up.ToList();
        spawnPool_down = sortedPool_down.ToList();
    }

}
