using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;

public class StateManager : Singleton<StateManager>
{

    [Serializable]
    public struct StateInfo
    {

        public float product_gold;
        public float product_metal;
        public float product_food;
        public float product_research;

        public int pop;
        public float growth_rating;
        public int growth_pop;

        public int jobs;
        public int unemployed;
        public int housing;
        public int emptyHousing;
    }

    [Header("설정값")]
    public BuildingInfo[] buildingInfos;
    public int totalState = 2;
    public float baseGrowthRating = 0.01f;
    public float cycleTime = 10f;

    [Header("게임 변수")]
    public StateInfo[] infos;

    public List<HaveBuildings> stateBuildings;
    [Serializable]
    public struct HaveBuildings
    {
        public List<int> list;
    }

    public Queue<BuildingQueueInfo> buildingQueue;
    [Serializable]
    public struct BuildingQueueInfo
    {
        public int stateIdx;
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
    public int total_unemployed;
    public float currentCycleTimer;

    private void Start()
    {
        infos = new StateInfo[totalState];

        stateBuildings = new List<HaveBuildings>();
        for (int i = 0; i < totalState; i++)
        {
            HaveBuildings haveBuildings = new HaveBuildings();
            haveBuildings.list = new List<int>();
            for (int n = 0; n < buildingInfos.Length; n++)
            {
                haveBuildings.list.Add(0);
            }
            stateBuildings.Add(haveBuildings);
        }

        buildingQueue = new Queue<BuildingQueueInfo>();
        currentBuildingTimer = int.MinValue;

        // 임시 초기 설정
        stateBuildings[0].list[0] = 1;
        infos[0].pop = 1000;

        YieldState(false);

        StartCoroutine(AAA());
    }

    private IEnumerator AAA()
    {
        yield return new WaitForSeconds(1f);

        BuildSomethingByBtn(0, 1);
        BuildSomethingByBtn(0, 0);
        BuildSomethingByBtn(0, 1);
    }

    private void Update()
    {
        if (currentCycleTimer >= cycleTime)
        {
            currentCycleTimer = 0f;
            YieldState(true);
        }
        else
        {
            currentCycleTimer += Time.deltaTime;
        }


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
                    currentBuildingTimer = buildingInfos[temp.buildingIdx].build_time;
                    Debug.Log("건설 대기열 불러옴 " + currentBuildingInfo.buildingIdx);
                }
            }
            else
            {
                BuildSomething(currentBuildingInfo.stateIdx, currentBuildingInfo.buildingIdx);
                currentBuildingTimer = int.MinValue;
                Debug.Log("건설 완료 " + currentBuildingInfo.buildingIdx);
            }
        }
    }


    public void BuildSomethingByBtn(int stateIdx, int buildingIdx)
    {
        if (GM.Instance.PayRes(buildingInfos[buildingIdx]))
        {
            BuildingQueueInfo queueInfo = new BuildingQueueInfo { buildingIdx = buildingIdx, stateIdx = stateIdx };
            if (currentBuildingTimer == int.MinValue)
            {
                currentBuildingInfo = queueInfo;
                currentBuildingTimer = buildingInfos[buildingIdx].build_time;
            }
            else
            {
                buildingQueue.Enqueue(queueInfo);
            }
        }
    }

    public void BuildSomething(int stateIdx, int buildingIdx, int count = 1)
    {
        stateBuildings[stateIdx].list[buildingIdx] += count;
        //CalcState(stateIdx);
        YieldState(false);
    }

    public void CalcState(int idx)
    {
        StateInfo temp = infos[idx];

        float product_gold = 0;
        float product_metal = 0;
        float product_food = 0;
        float product_research = 0;

        float growth_rating = 0;
        int jobs = 0;
        int housing = 0;

        for (int i = 0; i < stateBuildings[idx].list.Count; i++)
        {
            int count = stateBuildings[idx].list[i];
            var stat = buildingInfos[i];

            product_gold += stat.product_gold * count;
            product_metal += stat.product_metal * count;
            product_food += stat.product_food * count;
            product_research += stat.product_research * count;

            growth_rating += stat.growth_rating * count;
            jobs += stat.jobs * count;
            housing += stat.housing * count;
        }

        temp.product_gold = product_gold;
        temp.product_metal = product_metal;
        temp.product_food = product_food;
        temp.product_research = product_research;

        temp.jobs = jobs;
        temp.housing = housing;

        int unemployed = temp.pop - temp.jobs;
        temp.unemployed = unemployed;

        int emptyHousing = temp.housing - temp.pop;
        temp.emptyHousing = emptyHousing;

        float calcGrowthRating = baseGrowthRating + growth_rating;
        if (emptyHousing <= 0)
        {
            calcGrowthRating *= 0.5f;
        }
        if (unemployed > 0)
        {
            calcGrowthRating *= 0.5f;
        }
        temp.growth_rating = calcGrowthRating;

        float growth_pop = temp.pop * calcGrowthRating;
        growth_pop = Mathf.Max(growth_pop, 1f);
        temp.growth_pop = (int)growth_pop;

        infos[idx] = temp;
    }

    public void AddPop(int idx)
    {
        StateInfo temp = infos[idx];
        temp.pop += temp.growth_pop;
        infos[idx] = temp;
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
        total_unemployed = 0;

        for (int i = 0; i < totalState; i++)
        {
            CalcState(i);
            if (addRes)
                AddPop(i);

            total_product_gold += infos[i].product_gold;
            total_product_metal += infos[i].product_metal;
            total_product_food += infos[i].product_food;
            total_product_research += infos[i].product_research;

            total_pop += infos[i].pop;
            total_growth_pop += infos[i].growth_pop;
            total_housing += infos[i].housing;
            total_unemployed += infos[i].unemployed;
        }

        if (addRes)
        {
            GM.Instance.AddGold(total_product_gold);
            GM.Instance.AddMetal(total_product_metal);
            GM.Instance.AddFood(total_product_food);
        }
        GM.Instance.CalcResearch(total_product_research);

        GM.Instance.CalcPop(total_pop);
        GM.Instance.CalcHousing(total_housing);
        GM.Instance.CalcUnemployed(-1 * total_unemployed);
    }
}
