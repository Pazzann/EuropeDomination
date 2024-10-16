using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class ScenarioSettings
{
    public GameModes GameMode { get; set; }
    public float[] TimeScale { get; set; }
    public int[] DevForSpecialBuilding { get; set; }
    public int[] DevForCommonBuilding { get; set; }
    public int MaxRegimentWidthInBattle { get; set; }
    public float TaxEarningPerDev { get; set; }
    public float ManpowerPerDev { get; set; }
    public float InitialProduction { get; set; }
    public float MoraleRecoveryPerDay { get; set; }
    public List<KeyValuePair<int, double>> ResourceRequirmentsPer10Dev { get; set; }
    public int CostIncrementPerDev { get; set; }
    public float InitialMoneyCostColony { get; set; }
    public int InitialManpowerCostColony { get; set; }
    //add 3 to your value
    public int NavalColonizationRange { get; set; }
    public int ColonyGrowth { get; set; }
    public KeyValuePair<int, double[]> ResourceAndCostRequirmentsToDev(int dev)
    {
        var cost = dev * CostIncrementPerDev;
        var resources = Good.DefaultGoods(EngineState.MapInfo.Scenario.Goods.Count);
        //todo:fix
        for (var i = 0; i < Mathf.FloorToInt(dev / 10); i++)
            resources[ResourceRequirmentsPer10Dev[i].Key] += (dev - i * 10) * ResourceRequirmentsPer10Dev[i].Value;
        return new KeyValuePair<int, double[]>(cost, resources);
    }
    public float MoneyConsumptionPerMonthColony(int colonyNumber)
    {
        if (colonyNumber == 0) return 0;
        float initial = 2f;
        float sum = initial;
        for (int i = 1; i < colonyNumber; i++)
        {
            initial *= 2;
            sum += initial;
        }
        return sum;
    }
    public float MoneyConsumptionPerResearc(int researchNumber)
    {
        if (researchNumber == 0) return 0;
        float initial = 2f;
        float sum = initial;
        for (int i = 1; i < researchNumber; i++)
        {
            initial *= 2;
            sum += initial;
        }
        return sum;
    }

    public ScenarioSettings()
    {
        GameMode = GameModes.RandomSpawn;
        TimeScale = new []{0.5f, 0.4f, 0.3f, 0.2f, 0.1f};
        DevForSpecialBuilding = new int[3] { 10, 25, 50 };
        DevForCommonBuilding = new int[10] { 5, 10, 15, 20, 30, 40, 50, 60, 70, 80 };
        MaxRegimentWidthInBattle = 20;
        TaxEarningPerDev = 0.02f;
        ManpowerPerDev = 0.5f;
        InitialProduction = 1.0f;
        MoraleRecoveryPerDay = 0.01f;
        ResourceRequirmentsPer10Dev = new()
        {
            new KeyValuePair<int, double>(0, 0.25), // each index for 10 dev, value is to increment for dev
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5),
            new KeyValuePair<int, double>(2, 0.5)
        };
        CostIncrementPerDev = 10;
        InitialMoneyCostColony = 50;
        InitialManpowerCostColony = 100;
        NavalColonizationRange = 6;
        ColonyGrowth = 1000;
    }
}