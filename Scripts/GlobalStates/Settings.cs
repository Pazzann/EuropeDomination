using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Godot;

namespace EuropeDominationDemo.Scripts.GlobalStates;

public static class Settings
{
    public static GraphicPreset PerformaceMode = GraphicPreset.HighQualityMode;
    public static GameModes GameMode = GameModes.RandomSpawn;
    
    public static float[] TimeScale = new []{0.5f, 0.4f, 0.3f, 0.2f, 0.1f};
    
    public static int[] DevForSpecialBuilding = new int[3] { 10, 25, 50 };
    public static int[] DevForCommonBuilding = new int[10] { 5, 10, 15, 20, 30, 40, 50, 60, 70, 80 };
    public static int MaxRegimentWidthInBattle = 20;
    public static float TaxEarningPerDev = 0.02f;
    public static float ManpowerPerDev = 0.5f;
    public static float InitialProduction = 1.0f;
    public static float MoraleRecoveryPerDay = 0.01f;

    public static List<KeyValuePair<int, double>> ResourceRequirmentsPer10Dev = new()
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

    public static int CostIncrementPerDev = 10;

    public static KeyValuePair<int, double[]> ResourceAndCostRequirmentsToDev(int dev)
    {
        var cost = dev * CostIncrementPerDev;
        var resources = Good.DefaultGoods();
        //todo:fix
        for (var i = 0; i < Mathf.FloorToInt(dev / 10); i++)
            resources[ResourceRequirmentsPer10Dev[i].Key] += (dev - i * 10) * ResourceRequirmentsPer10Dev[i].Value;
        return new KeyValuePair<int, double[]>(cost, resources);
    }


    public static float InitialMoneyCostColony = 50;
    public static int InitialManpowerCostColony = 100;
    public static float MoneyConsumptionPerMonthColony(int colonyNumber)
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
    public static float MoneyConsumptionPerResearc(int researchNumber)
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
    public static int NavalColonizationRange = 6;
}