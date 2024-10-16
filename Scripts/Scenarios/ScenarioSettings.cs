using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;


//probably just move to scenario
public class ScenarioSettings
{
    public GameModes GameMode = GameModes.RandomSpawn;
    
    public float[] TimeScale = new []{0.5f, 0.4f, 0.3f, 0.2f, 0.1f};
    
    public int[] DevForSpecialBuilding = new int[3] { 10, 25, 50 };
    public int[] DevForCommonBuilding = new int[10] { 5, 10, 15, 20, 30, 40, 50, 60, 70, 80 };
    public int MaxRegimentWidthInBattle = 20;
    public float TaxEarningPerDev = 0.02f;
    public float ManpowerPerDev = 0.5f;
    public float InitialProduction = 1.0f;
    public float MoraleRecoveryPerDay = 0.01f;

    public List<KeyValuePair<int, double>> ResourceRequirmentsPer10Dev = new()
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

    public int CostIncrementPerDev = 10;

    public KeyValuePair<int, double[]> ResourceAndCostRequirmentsToDev(int dev)
    {
        var cost = dev * CostIncrementPerDev;
        var resources = Good.DefaultGoods(EngineState.MapInfo.Scenario.Goods.Count);
        //todo:fix
        for (var i = 0; i < Mathf.FloorToInt(dev / 10); i++)
            resources[ResourceRequirmentsPer10Dev[i].Key] += (dev - i * 10) * ResourceRequirmentsPer10Dev[i].Value;
        return new KeyValuePair<int, double[]>(cost, resources);
    }


    public float InitialMoneyCostColony = 50;
    public int InitialManpowerCostColony = 100;
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
    //add 3 to your value
    public int NavalColonizationRange = 6;

    public ScenarioSettings()
    {
        
    }
}