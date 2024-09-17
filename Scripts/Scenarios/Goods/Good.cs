using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public abstract class Good
{
    public int Id { get; }
    public Vector3 Color { get; }
    public string Name { get; }
    public float Cost { get; }
    
    public Good(int id, string name, Vector3 color, float cost)
    {
        Id = id;
        Name = name;
        Color = color;
        Cost = cost;
    }


    public static double[] DefaultGoods(Dictionary<int, double> notNullGoods = null)
    {
        var a = new double[] { 0, 0, 0, 0, 0 };
        if (notNullGoods == null) return a;
        foreach (var kvp in notNullGoods) a[kvp.Key] = kvp.Value;

        return a;
    }

    public static bool CheckIfMeetsRequirements(double[] availableGoods, double[] neededGoods)
    {
        for (var i = 0; i < availableGoods.Length; i++)
            if (availableGoods[i] - neededGoods[i] < 0)
                return false;

        return true;
    }

    public static double[] DecreaseGoodsByGoods(double[] availableGoods, double[] neededGoods)
    {
        for (var i = 0; i < availableGoods.Length; i++) availableGoods[i] -= neededGoods[i];

        return availableGoods;
    }

    public static bool IsDifferentFromNull(double[] resources)
    {
        foreach (var t in resources)
            if (t > 0)
                return true;

        return false;
    }
}